import React, { useState } from "react";
import { Button, Modal, Form } from "semantic-ui-react";

const AddFolderModal = (props) => {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");

  const closeModalHandler = () => {
    props.addfolder(name);
    setName("");
    setOpen(false);
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
      {open ? (
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
            <Form>
              <Form.Field>
                <label>Folder Name</label>
                <input
                  placeholder="Folder Name"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                />
              </Form.Field>
              <Button
                type="submit"
                onClick={() => {
                  closeModalHandler();
                }}
              >
                Submit
              </Button>
            </Form>
          </div>
        </Modal>
      ) : null}
    </div>
  );
};

export default AddFolderModal;
