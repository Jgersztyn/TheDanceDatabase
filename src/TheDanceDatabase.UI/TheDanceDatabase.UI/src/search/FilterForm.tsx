import { useRef, type SubmitEvent } from 'react';
import type { DanceSearchCriteria, DanceStyle } from '../api/danceTypes';

interface FilterFormProps {
  styles: DanceStyle[];
  filters: DanceSearchCriteria;
  onChange: (filters: DanceSearchCriteria) => void;
  onClear: () => void;
}

function FilterForm({ styles, filters, onChange, onClear }: FilterFormProps) {
  const { style, category, move } = filters;
  const categories = styles.find((candidate) => candidate.name === style)?.categories ?? [];
  const isFiltered = Boolean(style || category || move.trim());
  const styleSelect = useRef<HTMLSelectElement>(null);

  // Clearing disables the button, so hand focus to the first filter rather than lose it
  function handleClear() {
    onClear();
    styleSelect.current?.focus();
  }

  // Filters apply as they change, so Enter has nothing left to submit
  function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
  }

  return (
    <form className="filter-form card" role="search" onSubmit={handleSubmit}>
      <div className="field">
        <label htmlFor="filter-style">Style</label>
        <select
          id="filter-style"
          ref={styleSelect}
          value={style}
          onChange={(event) => onChange({ ...filters, style: event.target.value, category: '' })}
        >
          <option value="">All styles</option>
          {styles.map((option) => (
            <option key={option.id} value={option.name}>
              {option.name}
            </option>
          ))}
        </select>
      </div>

      <div className="field">
        <label htmlFor="filter-category">Category</label>
        <select
          id="filter-category"
          value={category}
          disabled={!style}
          onChange={(event) => onChange({ ...filters, category: event.target.value })}
        >
          <option value="">{style ? 'All categories' : 'Choose a style first'}</option>
          {categories.map((option) => (
            <option key={option.id} value={option.name}>
              {option.name}
            </option>
          ))}
        </select>
      </div>

      <div className="field">
        <label htmlFor="filter-move">Move name</label>
        <input
          id="filter-move"
          type="search"
          value={move}
          placeholder="Type to filter, e.g. kick"
          autoComplete="off"
          onChange={(event) => onChange({ ...filters, move: event.target.value })}
        />
      </div>

      <button
        className="button button-quiet"
        type="button"
        disabled={!isFiltered}
        onClick={handleClear}
      >
        Clear filters
      </button>
    </form>
  );
}

export default FilterForm;
