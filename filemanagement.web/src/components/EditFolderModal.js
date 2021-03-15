import React, { useContext, useState } from "react";
import { Icon, Modal, Form, Button } from "semantic-ui-react";

import { EDITFOLDER, FolderContext } from "../context/FolderContext";
import { editfolder } from "../services/folderService";

const EditFolderModal = (props) => {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState(props.name);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const { dispatch } = useContext(FolderContext);

  const editHandler = async () => {
    setError("");
    if (name !== "") {
      try {
        setIsLoading(true);
        const response = await editfolder(props.id, name);
        if (response.status < 400) {
          dispatch({
            type: EDITFOLDER,
            fid: props.id,
            name: name,
          });
        } else {
          setError(response);
        }
        setIsLoading(false);
        setOpen(false);
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
      <Icon
        name="edit"
        onClick={() => {
          setOpen(true);
        }}
      />

      <Modal
        onOpen={() => setOpen(true)}
        closeIcon
        open={open}
        size="small"
        onClose={() => {
          setOpen(false);
        }}
      >
        <h3 style={{ textAlign: "center", paddingBottom: 10 }}>
          Edit Folder Name
        </h3>
        <hr />
        <div style={{ margin: 20 }}>
          <Form
            onSubmit={() => {
              editHandler();
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

export default EditFolderModal;
