import React from 'react';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';

export const AthenaProfile = ({ data }) => {
    const { AGUserId, GoldDeluxeUserId, VoidbridgeUserId, ExchangeUserId,
        FirstName, LastName, Gender, Birthday, Phone, Country, Address, City, Town, State, ZipCode,
        SecurityQuestion, SecurityCode, HowToKnow, PreferredUserName, PreferredCurrency, PreferredOdds, PreferredLanguage } = data || {};

    return (
        <tbody>
            <tr>
                <td className="first-cell">{ClientResourceManager.getText('AgUserId')}</td>
                <td>{AGUserId}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('GoldDeluxeUserId')}</td>
                <td>{GoldDeluxeUserId}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('MacauGamesUserId')}</td>
                <td>{VoidbridgeUserId}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('ExchangeUserId')}</td>
                <td>{ExchangeUserId}</td>
            </tr>
            <tr>
                <td colSpan="2"></td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('FirstName')}</td>
                <td>{FirstName}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('LastName')}</td>
                <td>{LastName}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('Gender')}</td>
                <td>{Gender}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('DateOfBirth')}</td>
                <td>{Birthday}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('Mobile')}</td>
                <td>{Phone}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('Country')}</td>
                <td>{Country}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('Address')}</td>
                <td>{Address}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('City')}/ {ClientResourceManager.getText('Town')}</td>
                <td>{City}{City && Town && '/'}{Town}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('State')}</td>
                <td>{State}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('ZipCode')}</td>
                <td>{ZipCode}</td>
            </tr>
            <tr>
                <td colSpan="2"></td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('SecurityQuestion')}</td>
                <td>{SecurityQuestion}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('SecurityCode')}</td>
                <td>{SecurityCode}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('WhereHear')}</td>
                <td>{HowToKnow}</td>
            </tr>
            <tr>
                <td colSpan="2"></td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('PreferredUserName')}</td>
                <td>{PreferredUserName}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('PreferredCurrency')}</td>
                <td>{PreferredCurrency}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('PreferredOdds')}</td>
                <td>{PreferredOdds}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('PreferredLanguage')}</td>
                <td>{PreferredLanguage}</td>
            </tr>
        </tbody>
    );
}

export default AthenaProfile;