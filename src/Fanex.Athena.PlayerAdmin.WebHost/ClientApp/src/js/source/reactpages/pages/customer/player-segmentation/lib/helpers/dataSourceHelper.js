export default class DataSourceHelper {
    static CreateDataSourceFrom(originDataSource, selectedItems) {
        const selectedItemList = selectedItems.split(',').filter(x => x !== '');
        const convertedList = originDataSource.map(item => {
            const { key, value } = item;
            return {
                key,
                value,
                status: selectedItemList.find(_ => _ == key) ? true : false
            }
        })

        return convertedList;
    }
}