﻿/* Copyright (c) 2019-present Evereal. All rights reserved. */

using UnityEngine;
using UnityEngine.UI;

namespace Evereal.VideoCapture
{

    [RequireComponent(typeof(VideoCaptureManager))]
    public class VideoCaptureManagerGUI : MonoBehaviour
    {

        // SerializeField
        [SerializeField]
        Button recordStartBtn_Cp;

        [SerializeField]
        Button recordStopBtn_Cp;

        [SerializeField]
        Transform description_Tf;

        [SerializeField]
        Text descriptionText_Cp;

        private VideoCaptureManager videoCaptureManager;

        private void Awake()
        {
            videoCaptureManager = GetComponent<VideoCaptureManager>();
            Application.runInBackground = true;
        }

        private void OnEnable()
        {
            foreach (VideoCapture videoCapture in videoCaptureManager.videoCaptures)
            {
                videoCapture.OnComplete += HandleCaptureComplete;
                videoCapture.OnError += HandleCaptureError;
            }
        }

        private void OnDisable()
        {
            foreach (VideoCapture videoCapture in videoCaptureManager.videoCaptures)
            {
                videoCapture.OnComplete -= HandleCaptureComplete;
                videoCapture.OnError -= HandleCaptureError;
            }
        }

        private void HandleCaptureComplete(object sender, CaptureCompleteEventArgs args)
        {
            UnityEngine.Debug.Log("Save file to: " + args.SavePath);
        }

        private void HandleCaptureError(object sender, CaptureErrorEventArgs args)
        {
            //UnityEngine.Debug.Log(args.ErrorCode);
        }

        // private void OnGUI()
        // {
        //     if (GUI.Button(new Rect(Screen.width - 160, Screen.height - 60, 150, 50), "Browse"))
        //     {
        //         // Open video save directory
        //         Utils.BrowseFolder(videoCaptureManager.saveFolder);
        //     }
        //     bool stopped = false;
        //     bool pending = false;
        //     // check if still processing
        //     foreach (VideoCapture videoCapture in videoCaptureManager.videoCaptures)
        //     {
        //         if (videoCapture.status == CaptureStatus.STOPPED)
        //         {
        //             stopped = true;
        //             break;
        //         }
        //         if (videoCapture.status == CaptureStatus.PENDING)
        //         {
        //             pending = true;
        //             break;
        //         }
        //     }
        //     if (stopped)
        //     {
        //         if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Encoding"))
        //         {
        //             // Waiting processing end
        //         }
        //         return;
        //     }
        //     if (pending)
        //     {
        //         if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Muxing"))
        //         {
        //             // Waiting processing end
        //         }
        //         return;
        //     }
        //     if (videoCaptureManager.captureStarted)
        //     {
        //         if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Stop Capture"))
        //         {
        //             videoCaptureManager.StopCapture();
        //         }
        //         if (GUI.Button(new Rect(170, Screen.height - 60, 150, 50), "Cancel Capture"))
        //         {
        //             videoCaptureManager.CancelCapture();
        //         }
        //     }
        //     else
        //     {
        //         if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Start Capture"))
        //         {
        //             videoCaptureManager.StartCapture();
        //         }
        //     }
        // }

        public void OnClick_StartCapture()
        {
            if (videoCaptureManager.captureStarted)
            {
                return;
            }

            recordStartBtn_Cp.interactable = false;
            recordStopBtn_Cp.interactable = true;
            StartCoroutine(SetDescription("started"));

            videoCaptureManager.StartCapture();
        }

        public void OnClick_StopCapture()
        {
            if (!videoCaptureManager.captureStarted)
            {
                return;
            }

            recordStopBtn_Cp.interactable = false;
            recordStartBtn_Cp.interactable = true;
            StartCoroutine(SetDescription("finished"));

            videoCaptureManager.StopCapture();
        }

        IEnumerator SetDescription(string value)
        {
            descriptionText_Cp.text = value;
            description_Tf.SetActive(true);

            yield return new WaitForSeconds(3f);

            description_Tf.SetActive(false);
            descriptionText_Cp.text = string.Empty;
        }

        public void OnClick_CancelCapture()
        {
            if (!videoCaptureManager.captureStarted)
            {
                return;
            }

            videoCaptureManager.CancelCapture();
        }
        
        public void OnClick_BrowseCapture()
        {
            Utils.BrowseFolder(videoCaptureManager.saveFolder);
        }
    }

}