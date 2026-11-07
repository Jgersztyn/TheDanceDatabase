import { Link } from 'react-router';

const steps = [
  {
    emoji: '🎶',
    title: 'Pick a style',
    text: 'Start with the kind of dance that gets your feet tapping.',
  },
  {
    emoji: '🧭',
    title: 'Pick a category',
    text: 'Each style is split into categories, so you can focus on feet, arms or the floor.',
  },
  {
    emoji: '🕺',
    title: 'Learn a move',
    text: 'Choose a move, read the notes, and practise it until it feels like yours.',
  },
];

function HomePage() {
  return (
    <>
      <section className="hero">
        <p className="eyebrow">
          <span aria-hidden="true">🪩</span> Your dance floor starts here
        </p>
        <h1 className="display">Find your groove. Learn your next move.</h1>
        <p className="lead">
          The Dance Database collects dance styles, the techniques behind them and the moves that
          make them. Whether you have never danced before or you are hunting for a fresh trick,
          there is something here to get you moving.
        </p>
        <Link className="button" to="/search">
          Start exploring moves <span aria-hidden="true">→</span>
        </Link>
      </section>

      <section className="how-it-works" aria-labelledby="how-it-works-heading">
        <h2 id="how-it-works-heading">How it works</h2>
        <ol className="steps">
          {steps.map((step) => (
            <li className="step card" key={step.title}>
              <span className="step-emoji" aria-hidden="true">
                {step.emoji}
              </span>
              <h3>{step.title}</h3>
              <p>{step.text}</p>
            </li>
          ))}
        </ol>
        <p className="cta-row">
          <Link className="button button-secondary" to="/search">
            Search for a move <span aria-hidden="true">🔎</span>
          </Link>
        </p>
      </section>
    </>
  );
}

export default HomePage;
