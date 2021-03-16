import React, { useState } from "react";
import { Icon, Modal } from "semantic-ui-react";
import ReactPlayer from "react-player";

import { getsinglefile } from "../services/fileService";
import "../styles/fileViewer.css";
import Loader from "react-loader-spinner";

const VideoViewer = (props) => {
  const [open, setOpen] = useState(false);
  const [url, setUrl] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const videoOpenHandler = async (e) => {
    e.preventDefault();
    try {
      setIsLoading(true);
      const response = await getsinglefile(props.id);
      setUrl(URL.createObjectURL(response.data));
      setIsLoading(false);
    } catch (error) {
      console.log(error);
      setIsLoading(false);
    }
    setOpen(true);
  };

  return (
    <span>
      <Icon
        name="external square alternate"
        onClick={(e) => {
          videoOpenHandler(e);
        }}
      />
      <Modal
        basic
        closeIcon
        onOpen={() => {
          setOpen(true);
        }}
        onClose={() => {
          setOpen(false);
        }}
        open={open}
      >
        <div className="outer">
          <div className="inner">
            {isLoading ? (
              <Loader
                type="Circles"
                color="#00BFFF"
                height={100}
                width={100}
                visible={isLoading}
              />
            ) : (
              <ReactPlayer url={url} controls />
            )}
          </div>
        </div>
      </Modal>
    </span>
  );
};

export default VideoViewer;
