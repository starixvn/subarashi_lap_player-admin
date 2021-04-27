import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';

const BaseUserGuide = props => {
    const { lang, contents  } = props;
    const renderContentByLanguage = () => {
        if (lang === LangHelper.languageKeys.zhCN) {
            return contents.znCNContent();
        }
        return contents.engContent();
    }
    return <React.Fragment>
        {renderContentByLanguage()}
    </React.Fragment>;
}

export default BaseUserGuide;