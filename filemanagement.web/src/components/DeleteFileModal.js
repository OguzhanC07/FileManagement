import React, { useContext, useState } from "react";
import { Icon, Modal, Button } from "semantic-ui-react";

import { DELETEFILE, FileContext } from "../context/FileContext";
import { deletefile } from "../services/fileService";

const DeleteFileModal = (props) => {
  const [open, setOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { dispatch } = useContext(FileContext);

  const deleteHandler = async () => {
    setError("");
    try {
      setIsLoading(true);
      await deletefile(props.id);
      dispatch({
        type: DELETEFILE,
        fid: props.id,
      });
      setIsLoading(false);
      setOpen(false);
    } catch (error) {
      setError(error.message);
      setIsLoading(false);
    }
  };
  return (
    <div>
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
          Delete Folder
        </h3>

        <div style={{ textAlign: "center", paddingBottom: 10 }}>
          <p>Do you really want to delete this folder ?</p>
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
            onClick={() => deleteHandler()}
          >
            <Icon name="checkmark" /> Yes
          </Button>
        </div>
        <div style={{ margin: 20 }}>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
    </div>
  );
};

export default DeleteFileModal;
