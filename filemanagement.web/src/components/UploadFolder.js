import React, { useCallback, useState, useMemo } from "react";
import { useDropzone } from "react-dropzone";
import { Modal, Button, Icon } from "semantic-ui-react";

import { uploadfile } from "../services/fileService";

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

  const uploadHandler = () => {
    console.log(myFiles);
    uploadfile(14, myFiles);
    removeAll();
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
      {/* {files.length > 0 && <button onClick={removeAll}>Remove All</button>} */}
    </section>
  );
};

export default UploadFolder;
