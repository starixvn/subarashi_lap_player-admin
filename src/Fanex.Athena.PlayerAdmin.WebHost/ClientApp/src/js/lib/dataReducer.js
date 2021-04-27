export const updateObjectInArray = (array, indexItem, updatedItem) => {
    return array.map((item, index) => {
        if (index !== indexItem) {
            return item;
        }
        return {
            ...item,
            ...updatedItem
        };
    });
}