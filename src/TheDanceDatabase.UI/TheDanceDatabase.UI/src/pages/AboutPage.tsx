import { Link } from 'react-router';

interface Tier {
  emoji: string;
  title: string;
  text: string;
  exampleLabel: string;
  example: string;
  search: Record<string, string>;
}

// One real move followed from broad to specific, so each tier has a worked example.
// The names match the database exactly, because each example opens Dance Search filtered to it.
const tiers: Tier[] = [
  {
    emoji: '🎶',
    title: 'Style',
    text: 'A whole way of dancing, with its own feel and history.',
    exampleLabel: 'For example',
    example: 'Shuffling',
    search: { style: 'Shuffling' },
  },
  {
    emoji: '🧭',
    title: 'Category',
    text: 'A group of related techniques within a style, such as footwork.',
    exampleLabel: 'Inside Shuffling',
    example: 'Running-Man',
    search: { style: 'Shuffling', category: 'Running-Man' },
  },
  {
    emoji: '🕺',
    title: 'Move',
    text: 'One step or trick you can practise, sometimes with a note to help.',
    exampleLabel: 'Inside Running-Man',
    example: 'Reverse running man',
    search: { style: 'Shuffling', category: 'Running-Man', move: 'Reverse running man' },
  },
];

function AboutPage() {
  return (
    <article className="about">
      <header className="about-hero">
        <p className="eyebrow eyebrow-pill">About</p>
        <h1 className="display">About The Dance Database</h1>
        <p className="lead">
          The Dance Database is a growing collection of dance styles, the techniques inside them and
          the individual moves you can learn, all in one place.
        </p>
      </header>

      <section aria-labelledby="organised-heading">
        <div className="path-intro">
          <h2 id="organised-heading">How the moves are organised</h2>
          <p>Everything is arranged in three tiers, from broad to specific:</p>
        </div>
        <ol className="path">
          {tiers.map((tier, index) => (
            <li className="stop" key={tier.title}>
              <div className="stop-what">
                <h3>
                  <span aria-hidden="true">{tier.emoji}</span> {tier.title}
                </h3>
                <p>{tier.text}</p>
              </div>
              <span className="stop-node" aria-hidden="true">
                {index + 1}
              </span>
              <Link
                className="stop-example"
                to={{ pathname: '/search', search: `?${new URLSearchParams(tier.search)}` }}
              >
                <small>{tier.exampleLabel}</small>
                <b>{tier.example}</b>
                <span className="stop-example-hint">
                  See it in Dance Search <span aria-hidden="true">→</span>
                </span>
              </Link>
            </li>
          ))}
          <li className="stop stop-find">
            <div className="stop-what">
              <p>
                On the <Link to="/search">Dance Search</Link> page you can narrow things down tier
                by tier, or type part of a move name to find it anywhere.
              </p>
            </div>
            <span className="stop-node" aria-hidden="true">
              🔎
            </span>
            <div className="stop-action">
              <Link className="button" to="/search">
                Go to Dance Search <span aria-hidden="true">→</span>
              </Link>
            </div>
          </li>
        </ol>
      </section>

      <section className="note" aria-labelledby="beginners-heading">
        <h2 id="beginners-heading">
          New to dancing? <span aria-hidden="true">💖</span>
        </h2>
        <p>
          Everyone starts somewhere. Pick one move, play a song you love, and repeat it slowly until
          it clicks. It is fine to look awkward at first; that is what learning looks like. Small
          wins add up, and before long you will be stringing moves together.
        </p>
      </section>
    </article>
  );
}

export default AboutPage;
