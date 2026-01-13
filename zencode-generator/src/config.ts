export const BRIDGE_URL = import.meta.env.PROD
    ? window.location.origin
    : 'http://localhost:3005';
