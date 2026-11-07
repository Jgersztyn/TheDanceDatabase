import { useEffect, useRef, useState } from 'react';
import {
  apiBaseUrl,
  getDanceTypes,
  searchDanceTypes,
  type DanceSearchCriteria,
  type DanceStyle,
} from '../api/danceTypes';
import FilterForm from './FilterForm';
import SearchResults from './SearchResults';

// Long enough to skip the requests for each keystroke while typing, short enough to feel live
const moveFilterDelayMs = 250;

const noFilters: DanceSearchCriteria = { style: '', category: '', move: '' };

// filtersKey records which filters the results answer, so outdated results can be dimmed
type SearchState =
  | { status: 'loading' }
  | { status: 'error'; message: string }
  | { status: 'ready'; results: DanceStyle[]; filtersKey: string };

function hasFilters({ style, category, move }: DanceSearchCriteria): boolean {
  return Boolean(style.trim() || category.trim() || move.trim());
}

function keyOf({ style, category, move }: DanceSearchCriteria): string {
  return [style, category, move.trim()].join('\n');
}

function DanceSearchPage() {
  const [filters, setFilters] = useState<DanceSearchCriteria>(noFilters);
  const [debouncedMove, setDebouncedMove] = useState('');
  // Bumped by "Try again" so the same filters load again
  const [attempt, setAttempt] = useState(0);
  const [state, setState] = useState<SearchState>({ status: 'loading' });
  const [catalog, setCatalog] = useState<DanceStyle[]>([]);
  const catalogLoaded = useRef(false);

  const { style, category, move } = filters;

  useEffect(() => {
    const timer = setTimeout(() => setDebouncedMove(move), moveFilterDelayMs);
    return () => clearTimeout(timer);
  }, [move]);

  useEffect(() => {
    // Aborting on cleanup cancels the request when the filters change again or the page unmounts.
    const controller = new AbortController();
    const criteria = { style, category, move: debouncedMove };
    const filtered = hasFilters(criteria);
    const load = filtered
      ? searchDanceTypes(criteria, controller.signal)
      : getDanceTypes(controller.signal);

    load.then(
      (results) => {
        if (controller.signal.aborted) return;
        if (!filtered) fillCatalog(results);
        setState({ status: 'ready', results, filtersKey: keyOf(criteria) });
      },
      (error: unknown) => {
        if (controller.signal.aborted) return;
        setState({
          status: 'error',
          message: error instanceof Error ? error.message : 'Unknown error',
        });
      },
    );

    // A failed first load leaves the style list empty, so a later filtered load refills it.
    // Its own failure is ignored: the results or error already tell the user what happened.
    if (filtered && !catalogLoaded.current) {
      getDanceTypes(controller.signal).then(
        (styles) => {
          if (!controller.signal.aborted) fillCatalog(styles);
        },
        () => {},
      );
    }

    function fillCatalog(styles: DanceStyle[]) {
      catalogLoaded.current = true;
      setCatalog(styles);
    }

    return () => controller.abort();
  }, [style, category, debouncedMove, attempt]);

  // True while typing is paused for the delay or a newer request is in flight
  const isUpdating = state.status === 'ready' && state.filtersKey !== keyOf(filters);

  function retry() {
    setState({ status: 'loading' });
    setAttempt((current) => current + 1);
  }

  return (
    <>
      <header className="page-header">
        <h1 className="display">Dance Search</h1>
        <p className="lead">
          Pick a style and a category to see every move in it, then type to filter them down.{' '}
          <span aria-hidden="true">💃🕺</span>
        </p>
      </header>

      <FilterForm
        styles={catalog}
        filters={filters}
        onChange={setFilters}
        onClear={() => {
          setFilters(noFilters);
          setDebouncedMove('');
        }}
      />

      <section className="results" aria-label="Matching moves" aria-busy={isUpdating}>
        {state.status === 'loading' && (
          <div className="loading" role="status">
            <span className="spinner" aria-hidden="true" />
            Warming up the dance floor…
          </div>
        )}

        {state.status === 'error' && (
          <div className="error-state card" role="alert">
            <h2>We couldn't reach the dance API</h2>
            <p>
              Make sure the API is running at <code>{apiBaseUrl}</code>, then try again.
            </p>
            <p className="error-detail">Details: {state.message}</p>
            <button className="button" type="button" onClick={retry}>
              Try again
            </button>
          </div>
        )}

        {state.status === 'ready' && <SearchResults styles={state.results} />}
      </section>
    </>
  );
}

export default DanceSearchPage;
