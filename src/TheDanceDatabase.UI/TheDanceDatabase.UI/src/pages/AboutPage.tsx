import { Link } from 'react-router';

function AboutPage() {
  return (
    <article className="prose">
      <h1 className="display">About The Dance Database</h1>
      <p className="lead">
        The Dance Database is a growing collection of dance styles, the techniques inside them and
        the individual moves you can learn, all in one place.
      </p>

      <section aria-labelledby="organised-heading">
        <h2 id="organised-heading">How the moves are organised</h2>
        <p>Everything is arranged in three tiers, from broad to specific:</p>
        <ol className="tiers">
          <li className="card">
            <strong>
              <span aria-hidden="true">🎶</span> Style
            </strong>
            <span>A whole way of dancing, with its own feel and history.</span>
          </li>
          <li className="card">
            <strong>
              <span aria-hidden="true">🧭</span> Category
            </strong>
            <span>A group of related techniques within a style, such as footwork.</span>
          </li>
          <li className="card">
            <strong>
              <span aria-hidden="true">🕺</span> Move
            </strong>
            <span>One step or trick you can practise, sometimes with a note to help.</span>
          </li>
        </ol>
        <p>
          On the <Link to="/search">Dance Search</Link> page you can narrow things down tier by
          tier, or type part of a move name to find it anywhere.
        </p>
      </section>

      <section className="callout" aria-labelledby="beginners-heading">
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
