import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';

const BaseUserGuide = props => {
    const { lang, contents, ...rest  } = props;
    const renderContentByLanguage = () => {
        if (lang === LangHelper.languageKeys.zhCN) {
            return contents.znCNContent(rest);
        } else if (lang === LangHelper.languageKeys.viVN) {
            return contents.viVNContent(rest);
        }
        return contents.engContent(rest);
    }
    return <React.Fragment>
        {renderContentByLanguage()}
    </React.Fragment>;
}

export default BaseUserGuide;