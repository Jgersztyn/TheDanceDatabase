import { Link } from 'react-router';

function NotFoundPage() {
  return (
    <section className="not-found">
      <p className="not-found-code" aria-hidden="true">
        404
      </p>
      <h1 className="display">This page sat this one out</h1>
      <p className="lead">We couldn't find the page you were looking for.</p>
      <Link className="button" to="/">
        Back to the dance floor
      </Link>
    </section>
  );
}

export default NotFoundPage;
