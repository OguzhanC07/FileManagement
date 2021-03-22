import React, { useContext, useState } from "react";
import { Icon, Modal, Button } from "semantic-ui-react";

import { DELETEFOLDER, FolderContext } from "../context/FolderContext";
import { DELETEFILE, FileContext } from "../context/FileContext";
import { deletefolder } from "../services/folderService";
import { deletefile } from "../services/fileService";

const DeleteModal = (props) => {
  const [open, setOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { dispatch } = useContext(FolderContext);
  const { dispatch: fileDispatch } = useContext(FileContext);

  const deleteHandler = async (e) => {
    e.preventDefault();
    setError("");
    switch (props.type) {
      case "folder":
        try {
          setIsLoading(true);
          await deletefolder(props.id);
          setIsLoading(false);
          setOpen(false);
          dispatch({
            type: DELETEFOLDER,
            fid: props.id,
          });
        } catch (error) {
          setError(error.message);
          setIsLoading(false);
        }
        break;
      case "file":
        try {
          setIsLoading(true);
          await deletefile(props.id);
          setIsLoading(false);
          setOpen(false);
          fileDispatch({
            type: DELETEFILE,
            fid: props.id,
          });
        } catch (error) {
          setError(error.message);
          setIsLoading(false);
        }
        break;
      default:
        break;
    }
  };

  return (
    <span>
      <Icon
        name="delete"
        onClick={() => {
          setOpen(true);
        }}
      />
      <Modal
        onOpen={() => setOpen(true)}
        basic
        closeIcon
        open={open}
        size="small"
        onClose={() => {
          setOpen(false);
        }}
      >
        <h3 style={{ textAlign: "center", paddingBottom: 10 }}>
          Delete {props.type === "file" ? "File" : "Folder"}
        </h3>

        <div style={{ textAlign: "center", paddingBottom: 10 }}>
          <p>
            Do you really want to delete this{" "}
            {props.type === "file" ? "File" : "Folder"} ?
          </p>
          <Button
            style={{ marginRight: 10 }}
            basic
            color="red"
            inverted
            onClick={() => setOpen(false)}
          >
            <Icon name="remove" /> No
          </Button>
          <Button
            color="green"
            loading={isLoading}
            inverted
            onClick={(e) => deleteHandler(e)}
          >
            <Icon name="checkmark" /> Yes
          </Button>
        </div>
        <div style={{ margin: 20 }}>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
    </span>
  );
};

export default DeleteModal;
