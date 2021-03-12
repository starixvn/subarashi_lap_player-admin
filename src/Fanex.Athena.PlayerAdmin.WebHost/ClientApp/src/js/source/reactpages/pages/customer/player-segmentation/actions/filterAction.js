import { SET_SUBSIDIARY_LIST, SET_SUBSIDIARY_SEARCH } from './types';

export const setSubsidiaryList = subsidiaryList => {
    return {
        type: SET_SUBSIDIARY_LIST,
        data: subsidiaryList
    }
}

export const setSubsidiarySearch = subsidiarySearch => {
    return {
        type: SET_SUBSIDIARY_SEARCH,
        data: subsidiarySearch
    }
}