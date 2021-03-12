import React, { useCallback, useState } from 'react';
import LangHelper from '@jsroot/lib/langHelper';
import { decode } from '@jsroot/lib/htmlHelper';
import { useDropzone } from 'react-dropzone';

const MbToByte = 1000 * 1000;

const DropzoneUpload = (props) => {
    const { onSubmit, onClose, multiple, accept, children, isShowInnerZone } = props;
    const maxSize = props.maxSize * MbToByte;

    const [uploadedFiles, setUploadedFiles] = useState([]);

    const onDrop = useCallback(acceptedFiles => {
        setUploadedFiles([...uploadedFiles, ...acceptedFiles])
    })

    const onDropRejected = useCallback(rejectedFiles => {
        const rejectedFile = rejectedFiles[0];
        let errorMsg = decode(LangHelper.getSingleResource('UploadFileError'));
        if (_isInvalidExtension(rejectedFile)) {
            errorMsg = `${decode(LangHelper.getSingleResource('FileExtensionIncorrect'))} (${accept})`;
        } else if (_isInValidSize(rejectedFile)) {
            errorMsg = `${LangHelper.getSingleResource('FileUploadExceedSize')} ${props.maxSize}MB`;
        }
        alert(errorMsg);
    })

    const { getRootProps, getInputProps } = useDropzone({ onDrop, onDropRejected, multiple, accept, maxSize });

    const _removeFile = file => () => {
        const newFiles = [...uploadedFiles]
        newFiles.splice(newFiles.indexOf(file), 1)
        setUploadedFiles(newFiles)
    }

    const _isInValidSize = (file) => {
        return file.size > maxSize;
    }

    const _isInvalidExtension = (file) => {
        const acceptArray = accept.split(',');
        const isValid = acceptArray.some(_ => file.name.endsWith(_));
        return !isValid;
    }


    const _renderAcceptedFiles = () => {
        return <ul className="dropzone-row">{uploadedFiles.map(file => (
            <li key={file.path} className="file-item">
                <a className="file-close" onClick={_removeFile(file)}><i className="icon-close"></i></a>
                <span>{file.name}</span>
            </li>))}
        </ul>
    }

    const _isExistFiles = uploadedFiles && uploadedFiles.length > 0;

    return (
        <div className="dropzone">
            <a className="dropzone-close" onClick={() => onClose && onClose()}><i className="icon-close"></i></a>
            { isShowInnerZone &&
                (_isExistFiles ? _renderAcceptedFiles() :
                    <div className="dropzone-row">
                        <div {...getRootProps({ className: 'inner-dropzone' })}>
                            <input {...getInputProps()} />
                            <p>{LangHelper.getSingleResource('ClickOrDropHere')}</p>
                        </div>
                    </div>)
            }
            {
                isShowInnerZone && <div className="dropzone-row">
                    <button className="btn" disabled={!_isExistFiles} onClick={() => onSubmit && onSubmit(uploadedFiles)}>{LangHelper.getSingleResource('Submit')}</button>
                </div>
            }
            {children}
        </div>
    );
}

DropzoneUpload.defaultProps = {
    maxSize: 1,
    isShowInnerZone: true
}

export default React.memo(DropzoneUpload);