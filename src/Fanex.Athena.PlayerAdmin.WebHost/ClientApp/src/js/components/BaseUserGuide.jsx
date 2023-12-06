import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';

const BaseUserGuide = props => {
    const { lang, contents, ...rest } = props;
    const renderContentByLanguage = () => {
        switch (lang) {
            case LangHelper.languageKeys.zhCN:
                return contents.znCNContent(rest);

            case LangHelper.languageKeys.viVN:
                return contents.viVNContent(rest);

            case LangHelper.languageKeys.thTH:
                return contents.thTHContent(rest);

            case LangHelper.languageKeys.idID:
                return contents.idIDContent(rest);
            
            default:
                return contents.engContent(rest);
        }
    }
    return <React.Fragment>
        {renderContentByLanguage()}
    </React.Fragment>;
}

export default BaseUserGuide;