import { Link, NavLink, Outlet } from 'react-router';

function SiteLayout() {
  return (
    <>
      <a className="skip-link" href="#main">
        Skip to content
      </a>
      <header className="site-header">
        <nav className="site-nav" aria-label="Main">
          <Link className="brand" to="/">
            <span aria-hidden="true">💃</span> The Dance Database
          </Link>
          <ul className="nav-links">
            <li>
              <NavLink className="nav-link" to="/" end>
                Home
              </NavLink>
            </li>
            <li>
              <NavLink className="nav-link" to="/search">
                Dance Search
              </NavLink>
            </li>
            <li>
              <NavLink className="nav-link" to="/about">
                About
              </NavLink>
            </li>
          </ul>
        </nav>
      </header>
      <main id="main" className="page">
        <Outlet />
      </main>
      <footer className="site-footer">
        <p>
          Keep moving <span aria-hidden="true">✨</span>
        </p>
      </footer>
    </>
  );
}

export default SiteLayout;
