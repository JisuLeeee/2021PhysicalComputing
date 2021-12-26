#include <Adafruit_NeoPixel.h>

#define LED_PIN    2
#define LED_COUNT 30

Adafruit_NeoPixel strip(LED_COUNT, LED_PIN, NEO_GRBW + NEO_KHZ800);

void setup() {
  Serial.begin(9600);
  strip.begin();
}

void loop() {
  int sensor = analogRead(A0);
  sensor = map(sensor, 0, 1023, 0, 14);
  Serial.println(sensor);
  
  if (Serial.available() > 1){
    byte bgm = Serial.read();
    byte vol = Serial.read();
    uint32_t color = strip.gamma32(strip.ColorHSV(0, 0, 0));
    vol = map(vol, 0, 127, 0, LED_COUNT);
    
    if (bgm == '0')
    {
        color = strip.gamma32(strip.ColorHSV(65536/3, 255, 255));
    } 
    else if (bgm == '1')
    {
        color = strip.gamma32(strip.ColorHSV(0, 255, 255));
    }
    
    strip.fill(strip.Color(0, 0, 0), 0, LED_COUNT);
    strip.fill(color, 0, vol);
    strip.show();
  }
}
