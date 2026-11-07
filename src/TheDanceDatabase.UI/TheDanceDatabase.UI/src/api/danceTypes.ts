export interface DanceMove {
  id: number;
  name: string;
  notes: string | null;
}

export interface DanceCategory {
  id: number;
  name: string;
  moves: DanceMove[];
}

export interface DanceStyle {
  id: number;
  name: string;
  categories: DanceCategory[];
}

export interface DanceSearchCriteria {
  style: string;
  category: string;
  move: string;
}

const defaultApiBaseUrl = 'http://localhost:5185';

/** Where the API lives: `VITE_API_BASE_URL` at build time, else the local default, without a trailing slash. */
export const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL?.trim() || defaultApiBaseUrl).replace(
  /\/+$/,
  '',
);

async function getDanceStyles(path: string, signal: AbortSignal): Promise<DanceStyle[]> {
  const response = await fetch(`${apiBaseUrl}${path}`, {
    headers: { Accept: 'application/json' },
    signal,
  });
  if (!response.ok) {
    throw new Error(`GET ${path} returned ${response.status} ${response.statusText}`.trim());
  }

  const body: unknown = await response.json();
  if (!Array.isArray(body)) {
    throw new Error(`GET ${path} did not return a list of dance styles`);
  }
  return body as DanceStyle[];
}

/** Fetches every style with its categories and moves. */
export function getDanceTypes(signal: AbortSignal): Promise<DanceStyle[]> {
  return getDanceStyles('/dance-types', signal);
}

/** Fetches the styles that match the criteria; blank criteria are left out of the query. */
export function searchDanceTypes(
  criteria: DanceSearchCriteria,
  signal: AbortSignal,
): Promise<DanceStyle[]> {
  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(criteria)) {
    const trimmed = value.trim();
    if (trimmed) {
      params.set(key, trimmed);
    }
  }
  const query = params.toString();
  return getDanceStyles(`/dance-types/search${query ? `?${query}` : ''}`, signal);
}
