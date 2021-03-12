const DEFAULT_RESOURCE_KEY = 'en';
const resources = {
    'en': {},
    'cs': {}
}
const currentLangKey = window._langKey || DEFAULT_RESOURCE_KEY;

export default class ClientResourceManager {
    static register(langKey, resourceValue) {
        resources[langKey] = {...resources[langKey], ...resourceValue };
    }

    static getText(key) {
        const currentResource = resources[currentLangKey];

        return currentResource && (currentResource[key] || resources[DEFAULT_RESOURCE_KEY][key] || key);
    }
}