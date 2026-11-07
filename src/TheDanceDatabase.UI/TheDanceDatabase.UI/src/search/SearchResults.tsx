import type { DanceCategory, DanceStyle } from '../api/danceTypes';

interface SearchResultsProps {
  styles: DanceStyle[];
}

function countMoves(styles: DanceStyle[]): number {
  return styles.reduce(
    (total, style) =>
      total + style.categories.reduce((sum, category) => sum + category.moves.length, 0),
    0,
  );
}

function plural(count: number, word: string): string {
  return `${count} ${word}${count === 1 ? '' : 's'}`;
}

function CategorySection({ category }: { category: DanceCategory }) {
  return (
    <section className="category">
      <h3>{category.name}</h3>
      {category.moves.length === 0 ? (
        <p className="empty-category">
          No moves here yet <span aria-hidden="true">🌱</span>
        </p>
      ) : (
        <ul className="move-list">
          {category.moves.map((move) => (
            <li className="move-chip" key={move.id}>
              <span className="move-name">{move.name}</span>
              {move.notes && <span className="move-notes">{move.notes}</span>}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}

function SearchResults({ styles }: SearchResultsProps) {
  if (styles.length === 0) {
    return (
      <div className="empty-state card" role="status">
        <p className="empty-state-emoji" aria-hidden="true">
          🙈
        </p>
        <h2>No moves match those filters</h2>
        <p>Try another style or category, or fewer letters of the move name.</p>
      </div>
    );
  }

  return (
    <>
      <p className="result-summary" role="status">
        Showing <strong>{plural(countMoves(styles), 'move')}</strong> across{' '}
        {plural(styles.length, 'style')}
      </p>
      <div className="style-grid">
        {styles.map((style) => (
          <article className="style-card card" key={style.id}>
            <h2>{style.name}</h2>
            {style.categories.map((category) => (
              <CategorySection category={category} key={category.id} />
            ))}
          </article>
        ))}
      </div>
    </>
  );
}

export default SearchResults;
