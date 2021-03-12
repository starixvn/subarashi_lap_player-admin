import { dataGridHeaders } from '../constants';

export const getDataGridHeaders = showSubsidiary => {
    const fullHeaders = [...dataGridHeaders];
    if(!showSubsidiary) {
        const headerIndex = dataGridHeaders.findIndex(_ => _.key === 'subsidiary');
        fullHeaders.splice(headerIndex, 1);
    }

    return fullHeaders;
} 
