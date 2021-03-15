import React, { useCallback, useState, useMemo, useContext } from "react";
import { useDropzone } from "react-dropzone";
import { Modal, Button, Icon } from "semantic-ui-react";
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

const UploadFolder = (props) => {
  const [myFiles, setMyFiles] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const { folder } = useContext(FolderContext);
  const { dispatch } = useContext(FileContext);

  const onDrop = useCallback(
    (acceptedFiles) => {
      setMyFiles([...myFiles, ...acceptedFiles]);
    },
    [myFiles]
  );

  const {
    getRootProps,
    getInputProps,
    isDragActive,
    isDragAccept,
  } = useDropzone({
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
    <li style={{ listStyle: "none" }} key={file.path}>
      {file.path} - {file.size} bytes
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
            files: res.data,
          });
        });
        setIsLoading(false);
        removeAll();
      } catch (error) {
        setError(error.message);
        setIsLoading(false);
      }
    } else {
      try {
        setIsLoading(true);
        const response = await uploadfile(folder.folders[0].id, myFiles);
        console.log(response);
        getfiles(folder.folder[0].id).then((res) => {
          dispatch({
            type: SETFILES,
            files: res.data,
          });
        });
        setIsLoading(false);
        removeAll();
      } catch (error) {
        setError(error.message);
        setIsLoading(false);
      }
    }
  };
  return (
    <section>
      <div {...getRootProps({ style })}>
        <input {...getInputProps()} />
        <p>Drag 'n' drop some files here, or click to select files</p>
      </div>
      <Modal basic closeIcon onClose={removeAll} open={files.length > 0}>
        <h1 style={{ textAlign: "center", paddingBottom: 10 }}>File Upload</h1>
        <div style={{ textAlign: "center", paddingBottom: 10 }}>
          <p style={{ fontSize: 25 }}>You can remove unwanted files</p>
          {files}
          <Button onClick={removeAll}>Remove All</Button>
          <div style={{ paddingTop: 20 }}>
            <Button
              style={{ marginRight: 10 }}
              basic
              color="red"
              inverted
              onClick={removeAll}
            >
              <Icon name="remove" /> Cancel
            </Button>
            <Button
              color="green"
              loading={isLoading}
              inverted
              onClick={() => {
                uploadHandler();
              }}
            >
              <Icon name="checkmark" /> Upload Files
            </Button>
          </div>
        </div>
        <div style={{ margin: 20 }}>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
    </section>
  );
};

export default UploadFolder;
