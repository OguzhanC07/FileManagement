import React, { useState } from "react";
import { Icon, Modal } from "semantic-ui-react";

import "../styles/fileViewer.css";
import { getsinglefile } from "../services/fileService";

const ImgViewer = (props) => {
  const [open, setOpen] = useState(false);
  const [file, setFile] = useState("");

  const imgViewHandler = async (e) => {
    e.preventDefault();
    const response = await getsinglefile(props.id);
    const base64 = await convertBase64(response.data);
    setFile(base64);
    setOpen(true);
  };

  const convertBase64 = (blob) => {
    return new Promise((resolve, reject) => {
      const fileReader = new FileReader();
      fileReader.readAsDataURL(blob);

      fileReader.onload = () => {
        resolve(fileReader.result);
      };

      fileReader.onerror = (error) => {
        reject(error);
      };
    });
  };

  return (
    <span>
      <Icon
        name="external square alternate"
        onClick={(e) => {
          imgViewHandler(e);
        }}
      />
      <Modal
        basic
        closeIcon
        open={open}
        onOpen={() => {
          setOpen(true);
        }}
        onClose={() => {
          setOpen(false);
        }}
      >
        <div className="outer">
          <img src={file} height="400px" alt="your" />
        </div>
      </Modal>
    </span>
  );
};

export default ImgViewer;
