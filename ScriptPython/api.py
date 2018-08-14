import win32con
import win32api
import sys
import os
import time
import win32gui, win32ui
import numpy as np
import cv2
import threading
from enum import Enum

#按键枚举
class KeyValues(int):
    A = 65
    B = 66
    C = 67
    D = 68
    E = 69
    F = 70
    G = 71
    H = 72
    I = 73
    J = 74
    K = 75
    L = 76
    M = 77
    N = 78
    O = 79
    P = 80
    Q = 81
    R = 82
    S = 83
    T = 84
    U = 85
    V = 86
    W = 87
    X = 88
    Y = 89
    Z = 90



class moling(object):

    def __init__(self):
        self.clearTime = 120
        self.startTemp = "start.png"
        self.endTemp = "end.png"
        self.vectorTemp = "vector.png"
        self.failTemp = "fail.png"
        self.bar = "bar.png"
        self.getFW = "get.png"
        self.nlgTemp = "nlg.png"

        self.startGame = KeyValues.A
        self.returnKey = KeyValues.H
        self.loopKey = KeyValues.Z
        self.whiteKey = KeyValues.B
        self.sureKey = KeyValues.S
        self.getKey = KeyValues.G
        self.failKey = KeyValues.F
        self.nlgReturnKey = KeyValues.R
        self.getFWtow = KeyValues.P


    #清剧情模式
    def clearPlot(self, times, waitchecktime):
        for i in range(0, times):
            if self.__mathchTemp(self.startTemp) == True:
                print("检测到正在战斗开始界面，准备开始进入战斗!")
            else:
                print("检测到界面异常，退出脚本!")
                return
            self.__keydown(self.startGame)
            time.sleep(1.5)
            if self.__mathchTemp(self.bar) == True:
                print("检测到进度条，游戏开始进入战斗！")
            else:
                print("未知原因没有检测到进度条!")
            
            time.sleep(waitchecktime)
            if self.__getgameisvector() == False:
                print("游戏失败一次!")
                self.__keydown(self.whiteKey)
                self.__keydown(self.failKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.loopKey)
            else:
                print("游戏胜利一次!")
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                if self.__mathchTemp(self.getFW) == True:
                    self.__keydown(self.sureKey)
                else:
                    self.__keydown(self.getKey)
                self.__keydown(self.loopKey)
            time.sleep(2)
            if self.__mathchTemp(self.nlgTemp) == True:
                self.__keydown(self.nlgReturnKey)
                print("体力不足，脚本退出")
                return
            time.sleep(3)
        print("脚本完整运行结束")
    #刷龙模式
    def drogmode(self, times, waitchecktime):
        for i in range(0, times):
            if self.__mathchTemp(self.startTemp) == True:
                print("检测到正在战斗开始界面，准备开始进入战斗!")
            else:
                print("检测到界面异常，退出脚本!")
                return
            self.__keydown(self.startGame)
            time.sleep(1.5)
            if self.__mathchTemp(self.bar) == True:
                print("检测到进度条，游戏开始进入战斗！")
            else:
                print("未知原因没有检测到进度条!")
            
            time.sleep(waitchecktime)
            if self.__getgameisvector() == False:
                print("游戏失败一次!")
                self.__keydown(self.whiteKey)
                self.__keydown(self.failKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.loopKey)
            else:
                print("游戏胜利一次!")
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                self.__keydown(self.whiteKey)
                if self.__mathchTemp(self.getFW) == True:
                    self.__keydown(self.getFWtow)
                else:
                    self.__keydown(self.getKey)
                self.__keydown(self.loopKey)
            time.sleep(2)
            if self.__mathchTemp(self.nlgTemp) == True:
                self.__keydown(self.nlgReturnKey)
                print("体力不足，脚本退出")
                return
            time.sleep(3)
        print("脚本完整运行结束")

    #检测游戏是否
    def __getgameisvector(self):
        isover = False
        valuetimes = 0
        while isover == False:
            if self.__mathchTemp(self.vectorTemp) == True:
                print("一场游戏已经胜利准备开宝箱!")
                return True
            time.sleep(1)
            if self.__mathchTemp(self.failTemp) == True:
                print("检测到游戏失败")
                return False
            time.sleep(1)
            valuetimes += 1
            if valuetimes > 360:
                print("检测超时!")
                return False

        
    #检测当前屏幕中是否有该模板
    def __mathchTemp(self, temp):
        self.__window_capture()
        return self.__mathc_img(temp)


    #窗口截图
    def __window_capture(self, filename = "window.jpg"):
        hwnd = 0 # 窗口的编号，0号表示当前活跃窗口
        # 根据窗口句柄获取窗口的设备上下文DC（Divice Context）
        hwndDC = win32gui.GetWindowDC(hwnd)
        # 根据窗口的DC获取mfcDC
        mfcDC = win32ui.CreateDCFromHandle(hwndDC)
        # mfcDC创建可兼容的DC
        saveDC = mfcDC.CreateCompatibleDC()
        # 创建bigmap准备保存图片
        saveBitMap = win32ui.CreateBitmap()
        # 获取监控器信息
        MoniterDev = win32api.EnumDisplayMonitors(None, None)
        w = MoniterDev[0][2][2]
        h = MoniterDev[0][2][3]
        # print w,h　　　#图片大小
        # 为bitmap开辟空间
        saveBitMap.CreateCompatibleBitmap(mfcDC, w, h)
        # 高度saveDC，将截图保存到saveBitmap中
        saveDC.SelectObject(saveBitMap)
        # 截取从左上角（0，0）长宽为（w，h）的图片
        saveDC.BitBlt((0, 0), (w, h), mfcDC, (0, 0), win32con.SRCCOPY)
        saveBitMap.SaveBitmapFile(saveDC, filename)



    #图像匹配
    def __mathc_img(self, Target, image = "window.jpg" ,value = 0.8):
        img_rgb = cv2.imread(image)
        img_gray = cv2.cvtColor(img_rgb, cv2.COLOR_BGR2GRAY)
        template = cv2.imread(Target,0)
        w, h = template.shape[::-1]
        res = cv2.matchTemplate(img_gray,template,cv2.TM_CCOEFF_NORMED)
        threshold = value
        loc = np.where( res >= threshold)
        return len(loc[0]) != 0 and len(loc[1]) != 0
# 　　    for pt in zip(*loc[::-1]):
# 　　        cv2.rectangle(img_rgb, pt, (pt[0] + w, pt[1] + h), (7,249,151), 2)   
# 　　    cv2.imshow('Detected',img_rgb)
# 　　    cv2.waitKey(0)
# 　　    cv2.destroyAllWindows()

    #按键事件
    def __keydown(self, value):
        win32api.keybd_event(int(value), 0, win32con.KEYEVENTF_EXTENDEDKEY, 0)  # 按下enter，第一个元素13为enter的键位码
        time.sleep(0.3)
        win32api.keybd_event(int(value), 0, win32con.KEYEVENTF_KEYUP, 0) #松开enter
        time.sleep(2)
        
    def __keydown2(self, value):
        return
    
    def findwindow(self):
        hwd = win32gui.FindWindow(None, "夜神模拟器")

        print(hwd)

ml = moling()
# ml.findwindow()
print("开始进行脚本")
time.sleep(7)
print("进入任务")
ml.clearPlot(105, 40)
print("脚本运行结束")


# print(int(KeyValues.S))
        
