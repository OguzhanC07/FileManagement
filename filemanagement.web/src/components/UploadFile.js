import React, { useCallback, useState, useMemo, useContext } from "react";
import { useDropzone } from "react-dropzone";
import { Modal, Button, Icon } from "semantic-ui-react";
import "react-toastify/dist/ReactToastify.css";
import { useTranslation } from "react-i18next";
import { toast, ToastContainer } from "react-toastify";

import { FileContext, SETFILES } from "../context/FileContext";
import { FolderContext } from "../context/FolderContext";
import { uploadfile, getfiles } from "../services/fileService";

const baseStyle = {
  flex: 1,
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
  padding: "20px",
  borderWidth: 2,
  borderRadius: 2,
  borderColor: "#eeeeee",
  borderStyle: "dashed",
  backgroundColor: "#fafafa",
  color: "#bdbdbd",
  outline: "none",
  transition: "border .24s ease-in-out",
};

const activeStyle = {
  borderColor: "#2196f3",
};

const acceptStyle = {
  borderColor: "#00e676",
};

const UploadFile = () => {
  const [myFiles, setMyFiles] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const { t } = useTranslation();

  const { folder } = useContext(FolderContext);
  const { dispatch } = useContext(FileContext);

  const onDrop = useCallback(
    (acceptedFiles) => {
      if (acceptedFiles.length > 50 || acceptedFiles.length === 0) {
        toast.error(t("uploadFile.uploadFileError"));
      } else {
        setMyFiles([...myFiles, ...acceptedFiles]);
      }
    },
    [myFiles, t]
  );

  const {
    getRootProps,
    getInputProps,
    isDragActive,
    isDragAccept,
  } = useDropzone({
    maxFiles: 50,
    onDrop,
  });

  const removeFile = (file) => () => {
    const newFiles = [...myFiles];
    newFiles.splice(newFiles.indexOf(file), 1);
    setMyFiles(newFiles);
  };

  const removeAll = () => {
    setMyFiles([]);
  };

  const files = myFiles.map((file) => (
    <li style={{ listStyle: "none", marginTop: 5 }} key={file.path}>
      {file.path} - {file.size} {t("uploadFile.byte")}
      <Icon
        style={{ marginLeft: 10, fontSize: 15 }}
        onClick={removeFile(file)}
        name="cancel"
      />
    </li>
  ));

  const style = useMemo(
    () => ({
      ...baseStyle,
      ...(isDragActive ? activeStyle : {}),
      ...(isDragAccept ? acceptStyle : {}),
    }),
    [isDragActive, isDragAccept]
  );

  const uploadHandler = async () => {
    setError("");
    if (folder.folderId > 0) {
      try {
        setIsLoading(true);
        await uploadfile(folder.folderId, myFiles);
        getfiles(folder.folderId).then((res) => {
          dispatch({
            type: SETFILES,
            files: res.data.data,
          });
        });
        setIsLoading(false);
        removeAll();
      } catch (err) {
        setError(t("uploadFile.uploadError") + t("responseErrors.wentWrong"));
        setIsLoading(false);
      }
    } else {
      try {
        setIsLoading(true);
        await uploadfile(folder.folders[0].id, myFiles);
        setIsLoading(false);
        removeAll();
      } catch (err) {
        setError(t("uploadFile.uploadError") + t("responseErrors.wentWrong"));
        setIsLoading(false);
      }
    }
  };

  return (
    <section>
      <div {...getRootProps({ style })}>
        <input {...getInputProps()} />
        <p>{t("uploadFile.dragnDropText")}</p>
      </div>
      <Modal basic closeIcon onClose={removeAll} open={files.length > 0}>
        <h1 style={{ textAlign: "center", paddingBottom: 10 }}>
          {t("uploadFile.fileUploadText")}
        </h1>
        <div style={{ textAlign: "center", paddingBottom: 10 }}>
          <p style={{ fontSize: 25 }}>{t("uploadFile.removeInfo")}</p>
          {files}
          <Button style={{ marginTop: 10 }} onClick={removeAll}>
            {t("uploadFile.removeAllBtn")}
          </Button>
          <div style={{ paddingTop: 20 }}>
            <Button
              style={{ marginRight: 10 }}
              basic
              color="red"
              inverted
              onClick={removeAll}
            >
              <Icon name="remove" /> {t("uploadFile.cancelBtn")}
            </Button>
            <Button
              color="green"
              loading={isLoading}
              inverted
              onClick={() => {
                uploadHandler();
              }}
            >
              <Icon name="checkmark" /> {t("uploadFile.uploadFileBtn")}
            </Button>
          </div>
        </div>
        <div style={{ margin: 20 }}>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
      <ToastContainer />
    </section>
  );
};

export default UploadFile;
