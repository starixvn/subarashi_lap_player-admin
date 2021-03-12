import { headerText, winLossText } from './constants';

export const generateHeaderText = (hasCustomerComm) => {
    const winLossColumn = hasCustomerComm ? winLossText : headerText.CustomerWinLoss;

    return { ...headerText, WinLoss: winLossColumn };
}