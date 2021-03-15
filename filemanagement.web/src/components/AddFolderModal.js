import React, { useContext, useState } from "react";
import { Button, Modal, Form } from "semantic-ui-react";

import { FolderContext, SETFOLDERS } from "../context/FolderContext";
import { getfolders, addfolders } from "../services/folderService";

const AddFolderModal = () => {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { folder, dispatch } = useContext(FolderContext);

  const addFolderModalHandler = async () => {
    setError("");
    if (name !== "") {
      try {
        setIsLoading(true);
        await addfolders(name, folder.folderId);
        getfolders(folder.folderId)
          .then((res) => {
            dispatch({
              type: SETFOLDERS,
              folders: res.data.data,
            });
            setOpen(false);
          })
          .catch((err) => {
            console.log(err.message);
            setOpen(false);
          });
        setName("");
        setIsLoading(false);
      } catch (error) {
        setError(error.message);
        setIsLoading(false);
      }
    } else {
      setError("Klasör adı boş olamaz");
    }
  };

  return (
    <div>
      <Button
        primary
        onClick={() => {
          setOpen(true);
        }}
      >
        Add Folder
      </Button>
      <Modal
        onOpen={() => setOpen(true)}
        closeIcon
        open={open}
        size="small"
        onClose={() => {
          setOpen(false);
        }}
      >
        <h3 style={{ textAlign: "center", paddingBottom: 10 }}>Add Folder</h3>
        <hr />
        <div style={{ margin: 20 }}>
          <Form
            onSubmit={() => {
              addFolderModalHandler();
            }}
          >
            <Form.Field>
              <label>Folder Name</label>
              <input
                placeholder="Folder Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
              />
            </Form.Field>
            <Button type="submit" loading={isLoading}>
              Submit
            </Button>
          </Form>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
    </div>
  );
};

export default AddFolderModal;
