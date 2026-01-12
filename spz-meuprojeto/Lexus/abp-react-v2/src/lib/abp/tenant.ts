/**
 * Utility to resolve tenant from the current domain/subdomain.
 * Inspired by ABP's DomainTenantResolveContributor.
 */

export const resolveTenantFromHostname = (): string | null => {
    const hostname = window.location.hostname;
    const format = import.meta.env.VITE_TENANT_DOMAIN_FORMAT; // e.g., "{0}.zensuite.com.br"

    if (!format || !format.includes("{0}")) {
        return null;
    }

    // Escape special regex characters except {0}
    const escapedFormat = format.replace(/[.*+?^${}()|[\]\\]/g, (match: string) => {
        return match === "{" || match === "}" || match === "0" ? match : `\\${match}`;
    });

    // Replace {0} with a capture group for the tenant name
    const regexPattern = `^${escapedFormat.replace("{0}", "([a-z0-9-]+)")}$`;
    const regex = new RegExp(regexPattern, "i");

    const match = hostname.match(regex);
    const resolved = match ? match[1] : null;

    if (resolved) {
        console.log(`[Tenant Resolver] Resolved tenant "${resolved}" from hostname "${hostname}" using format "${format}"`);
    }

    return resolved;
};

/**
 * Checks if the current application is running on the root domain 
 * (no tenant resolved from domain).
 */
export const isRootDomain = (): boolean => {
    return resolveTenantFromHostname() === null;
};
