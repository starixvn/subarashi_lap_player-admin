import React from 'react';
import AppLayout from '../../../../layouts/AppLayout';
import TabContent from './TabContent';
import WidgetContent from './WidgetContent';


const AppContainer = (props) => {
    return (
        <AppLayout>
            <div className="account-info-container">
                <TabContent />
                <WidgetContent />
            </div>
        </AppLayout>
    );
}

export default AppContainer;