import { SET_LRF_FILTERS } from './types';
import LrfFiltersService from '../lib/services/lrfFiltersService';

export const getLRFFilters = () => (dispatch) => {
    const service = new LrfFiltersService();

    service.fetchData().then((response) => {
        dispatch(setLRFFilters(response.data));
    }).catch(() => {
        dispatch(setLRFFilters({
            CustomerClass: [],
            CommGroups: []
        }));
    });
};

const setLRFFilters = (data) => {
    return {
        type: SET_LRF_FILTERS,
        data
    }
};
