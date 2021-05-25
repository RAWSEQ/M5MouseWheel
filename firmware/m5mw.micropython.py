from m5stack import *
from m5stack_ui import *
from uiflow import *
from ble import ble_uart
import face

screen = M5Screen()
screen.clean_screen()
screen.set_screen_bg_color(0x000000)


mb_click = None
lb_click = None
rb_click = None
snd_val = None
st_mode = None
stval = None
prval = None

faces_encode = face.get(face.ENCODE)
direction = M5Label('M5MouseWheel - Please dont touch for processing...', x=0, y=228, color=0xc7c7c7, font=FONT_MONT_12, parent=None)
LBtn = M5Btn(text='L', x=170, y=6, w=65, h=100, bg_c=0x000000, text_c=0xbcbcbc, font=FONT_UNICODE_24, parent=None)
RBtn = M5Btn(text='R', x=240, y=6, w=70, h=48, bg_c=0x000000, text_c=0xbebebe, font=FONT_UNICODE_24, parent=None)
d_scr_x = M5Btn(text='W X', x=0, y=162, w=100, h=48, bg_c=0x000000, text_c=0xd4d4d4, font=FONT_UNICODE_24, parent=None)
MBtn = M5Btn(text='M', x=240, y=58, w=70, h=48, bg_c=0x000000, text_c=0xbebebe, font=FONT_UNICODE_24, parent=None)
b_step = M5Btn(text='STEP', x=0, y=6, w=100, h=100, bg_c=0x000000, text_c=0xd4d4d4, font=FONT_UNICODE_24, parent=None)
d_y = M5Btn(text='Y', x=220, y=110, w=100, h=100, bg_c=0x000000, text_c=0xd4d4d4, font=FONT_MONT_48, parent=None)
d_scr = M5Btn(text='W Y', x=0, y=110, w=100, h=48, bg_c=0x000000, text_c=0xd4d4d4, font=FONT_UNICODE_24, parent=None)
d_x = M5Btn(text='X', x=110, y=110, w=100, h=100, bg_c=0x000000, text_c=0xd4d4d4, font=FONT_MONT_48, parent=None)
v_step = M5Label('1', x=121, y=38, color=0xc7c7c7, font=FONT_MONT_24, parent=None)


# Change Mode
def changeMode():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  snd_val = 0
  uart_ble.write((str(st_mode) + str(str(snd_val))))
  direction.set_text(str((str(st_mode) + str(str(snd_val)))))

# Reset Mode
def resetMode():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  st_mode = ''
  b_step.set_bg_color(0x000000)
  d_y.set_bg_color(0x000000)
  d_scr.set_bg_color(0x000000)
  d_scr_x.set_bg_color(0x000000)
  d_x.set_bg_color(0x000000)


def LBtn_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  lb_click = 0 if lb_click == 1 else 1
  uart_ble.write((str('L') + str(str(lb_click))))
  if lb_click == 1:
    LBtn.set_bg_color(0x666666)
  else:
    LBtn.set_bg_color(0x000000)
  direction.set_text(str((str('L') + str(str(lb_click)))))
  pass
LBtn.pressed(LBtn_pressed)

def d_x_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  if st_mode != 'X':
    resetMode()
    st_mode = 'X'
    d_x.set_bg_color(0x666666)
    faces_encode.setLed(0, 0xff0000)
    changeMode()
  pass
d_x.pressed(d_x_pressed)

def RBtn_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  rb_click = 0 if rb_click == 1 else 1
  uart_ble.write((str('R') + str(str(rb_click))))
  if rb_click == 1:
    RBtn.set_bg_color(0x666666)
  else:
    RBtn.set_bg_color(0x000000)
  direction.set_text(str((str('R') + str(str(rb_click)))))
  pass
RBtn.pressed(RBtn_pressed)

def b_step_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  if st_mode != 'T':
    resetMode()
    st_mode = 'T'
    b_step.set_bg_color(0x666666)
    faces_encode.setLed(0, 0xffffff)
    changeMode()
  pass
b_step.pressed(b_step_pressed)

def d_scr_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  if st_mode != 'S':
    resetMode()
    st_mode = 'S'
    d_scr.set_bg_color(0x666666)
    faces_encode.setLed(0, 0xff9900)
    changeMode()
  pass
d_scr.pressed(d_scr_pressed)

def MBtn_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  mb_click = 0 if mb_click == 1 else 1
  uart_ble.write((str('M') + str(str(mb_click))))
  if mb_click == 1:
    MBtn.set_bg_color(0x666666)
  else:
    MBtn.set_bg_color(0x000000)
  direction.set_text(str((str('M') + str(str(mb_click)))))
  pass
MBtn.pressed(MBtn_pressed)

def d_y_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  if st_mode != 'Y':
    resetMode()
    st_mode = 'Y'
    d_y.set_bg_color(0x666666)
    faces_encode.setLed(0, 0x3333ff)
    changeMode()
  pass
d_y.pressed(d_y_pressed)

def d_scr_x_pressed():
  global lb_click, rb_click, mb_click, snd_val, st_mode, stval, prval
  if st_mode != 'U':
    resetMode()
    st_mode = 'U'
    d_scr_x.set_bg_color(0x666666)
    faces_encode.setLed(0, 0x33ff33)
    changeMode()
  pass
d_scr_x.pressed(d_scr_x_pressed)


resetMode()
uart_ble = ble_uart.init('m5mw_01')
stval = 1
st_mode = 'S'
prval = faces_encode.getValue()
snd_val = 0
d_scr.set_bg_color(0x666666)
faces_encode.setLed(0, 0xff9900)
uart_ble.write((str(st_mode) + str(str(snd_val))))
direction.set_text(str((str(st_mode) + str(str(snd_val)))))
while True:
  if (faces_encode.getValue()) != prval:
    if st_mode == 'T':
      stval = stval + ((faces_encode.getValue()) - prval)
      v_step.set_text(str(stval))
    else:
      snd_val = snd_val + ((faces_encode.getValue()) - prval) * stval
      uart_ble.write((str(st_mode) + str(str(snd_val))))
      direction.set_text(str((str(st_mode) + str(str(snd_val)))))
    prval = faces_encode.getValue()
  wait_ms(2)
