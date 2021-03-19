import React, { useState } from "react";
import { Modal, Icon } from "semantic-ui-react";

import { getsinglefile } from "../services/fileService";
import "../styles/fileViewer.css";
import ImgDisplay from "./ImgDisplay";
import PdfDisplay from "./PdfDisplay";
import VideoDisplay from "./VideoDisplay";

const Viewer = (props) => {
  const [file, setFile] = useState(null);
  const [open, setOpen] = useState(false);

  const viewHandler = async (e) => {
    e.preventDefault();
    const response = await getsinglefile(props.id);
    let base64;
    switch (props.type) {
      case "pdf":
        base64 = await convertBase64(response.data);
        setFile(base64);
        setOpen(true);
        break;
      case "img":
        base64 = await convertBase64(response.data);
        setFile(base64);
        setOpen(true);
        break;
      case "video":
        setFile(URL.createObjectURL(response.data));
        setOpen(true);
        break;
      default:
        break;
    }
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
          viewHandler(e);
        }}
      />
      <Modal
        onOpen={() => setOpen(true)}
        basic
        closeIcon
        open={open}
        onClose={() => {
          setOpen(false);
        }}
      >
        {props.type === "pdf" && <PdfDisplay file={file} />}
        {props.type === "img" && <ImgDisplay file={file} />}
        {props.type === "video" && <VideoDisplay file={file} />}
      </Modal>
    </span>
  );
};

export default Viewer;
