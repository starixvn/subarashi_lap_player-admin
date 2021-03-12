export const getFormattedDate = (dateString) => {
    const dateParts = dateString.split('/');

    return `${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`;
}