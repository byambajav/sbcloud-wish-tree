#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand sCmd;

const int ledPin = 13;
const int analogInPin = A0;
int   ad;



void setup()
{
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);
  
  while (!Serial);
  sCmd.addCommand("PING", pingHandler);
}

void loop()
{
  // reading human sensor
  ad = analogRead(analogInPin);
  if ( ad == 0 ) {
    digitalWrite(ledPin, LOW);
    Serial.println(" ");
  }else{
    digitalWrite(ledPin, HIGH);
    Serial.println("1");
  }
  
  // serial communication to unity
  if (Serial.available() > 0){
    sCmd.readSerial();
  }
}

void pingHandler () {
  Serial.println("PONG");
}

