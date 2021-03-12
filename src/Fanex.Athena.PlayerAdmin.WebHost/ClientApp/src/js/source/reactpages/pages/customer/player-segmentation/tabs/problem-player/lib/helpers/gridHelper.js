import { dataGridFullHeaders } from '../constants';

export const getDataGridHeaders = (hasVipManagement, showSubsidiary) => {
    const fullHeaders = [...dataGridFullHeaders];
    if (!hasVipManagement) {
        const headerIndex = dataGridFullHeaders.findIndex(_ => _.key === 'vip');
        fullHeaders.splice(headerIndex, 1);
    }

    if(!showSubsidiary) {
        const headerIndex = dataGridFullHeaders.findIndex(_ => _.key === 'subsidiary');
        fullHeaders.splice(headerIndex, 1);
    }

    return fullHeaders;
} 
