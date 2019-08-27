using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	class D68_MPU9250 : D00_BASE
	{
		int busadr = 0x00;
		int devadr = 0x68;
		//---

		public D68_MPU9250(int busadr)
		{
			this.busadr = busadr;
		}
		public override int BUSADR {
			get { return(this.busadr);}
		}
		public override int DEVADR {
			get { return(this.devadr);}
		}
		//---
		const int AHRS = 0;
		//---
		const int SELF_TEST_X_GYRO =0x00;
		const int SELF_TEST_Y_GYRO =0x01;
		const int SELF_TEST_Z_GYRO =0x02;

/*		const int X_FINE_GAIN      =0x03; // [7:0] fine gain
		const int Y_FINE_GAIN      =0x04;
		const int Z_FINE_GAIN      =0x05;
		const int XA_OFFSET_H      =0x06;// User-defined trim values for accelerometer
		const int XA_OFFSET_L_TC   =0x07;
		const int YA_OFFSET_H      =0x08;
		const int YA_OFFSET_L_TC   =0x09;
		const int ZA_OFFSET_H      =0x0A;
		const int ZA_OFFSET_L_TC   =0x0B; */

		const int SELF_TEST_X_ACCEL =0x0D;
		const int SELF_TEST_Y_ACCEL =0x0E;
		const int SELF_TEST_Z_ACCEL =0x0F;

		const int SELF_TEST_A       =0x10;

		const int XG_OFFSET_H       =0x13;  // User-defined trim values for gyroscope
		const int XG_OFFSET_L       =0x14;
		const int YG_OFFSET_H       =0x15;
		const int YG_OFFSET_L       =0x16;
		const int ZG_OFFSET_H       =0x17;
		const int ZG_OFFSET_L       =0x18;
		const int SMPLRT_DIV        =0x19;
		const int CONFIG            =0x1A;
		const int GYRO_CONFIG       =0x1B;
		const int ACCEL_CONFIG      =0x1C;
		const int ACCEL_CONFIG2     =0x1D;
		const int LP_ACCEL_ODR      =0x1E;
		const int WOM_THR           =0x1F;

// Duration counter threshold for motion interrupt generation, 1 kHz rate,
// LSB = 1 ms
		const int MOT_DUR           =0x20;
// Zero-motion detection threshold bits [7:0]
		const int ZMOT_THR          =0x21;
// Duration counter threshold for zero motion interrupt generation, 16 Hz rate,
// LSB = 64 ms
		const int ZRMOT_DUR         =0x22;

		const int FIFO_EN            =0x23;
		const int I2C_MST_CTRL       =0x24;
		const int I2C_SLV0_ADDR      =0x25;
		const int I2C_SLV0_REG       =0x26;
		const int I2C_SLV0_CTRL      =0x27;
		const int I2C_SLV1_ADDR      =0x28;
		const int I2C_SLV1_REG       =0x29;
		const int I2C_SLV1_CTRL      =0x2A;
		const int I2C_SLV2_ADDR      =0x2B;
		const int I2C_SLV2_REG       =0x2C;
		const int I2C_SLV2_CTRL      =0x2D;
		const int I2C_SLV3_ADDR      =0x2E;
		const int I2C_SLV3_REG       =0x2F;
		const int I2C_SLV3_CTRL      =0x30;
		const int I2C_SLV4_ADDR      =0x31;
		const int I2C_SLV4_REG       =0x32;
		const int I2C_SLV4_DO        =0x33;
		const int I2C_SLV4_CTRL      =0x34;
		const int I2C_SLV4_DI        =0x35;
		const int I2C_MST_STATUS     =0x36;
		const int INT_PIN_CFG        =0x37;
		const int INT_ENABLE         =0x38;
		const int DMP_INT_STATUS     =0x39;  // Check DMP interrupt
		const int INT_STATUS         =0x3A;
		const int ACCEL_XOUT_H       =0x3B;
		const int ACCEL_XOUT_L       =0x3C;
		const int ACCEL_YOUT_H       =0x3D;
		const int ACCEL_YOUT_L       =0x3E;
		const int ACCEL_ZOUT_H       =0x3F;
		const int ACCEL_ZOUT_L       =0x40;
		const int TEMP_OUT_H         =0x41;
		const int TEMP_OUT_L         =0x42;
		const int GYRO_XOUT_H        =0x43;
		const int GYRO_XOUT_L        =0x44;
		const int GYRO_YOUT_H        =0x45;
		const int GYRO_YOUT_L        =0x46;
		const int GYRO_ZOUT_H        =0x47;
		const int GYRO_ZOUT_L        =0x48;
		const int EXT_SENS_DATA_00   =0x49;
		const int EXT_SENS_DATA_01   =0x4A;
		const int EXT_SENS_DATA_02   =0x4B;
		const int EXT_SENS_DATA_03   =0x4C;
		const int EXT_SENS_DATA_04   =0x4D;
		const int EXT_SENS_DATA_05   =0x4E;
		const int EXT_SENS_DATA_06   =0x4F;
		const int EXT_SENS_DATA_07   =0x50;
		const int EXT_SENS_DATA_08   =0x51;
		const int EXT_SENS_DATA_09   =0x52;
		const int EXT_SENS_DATA_10   =0x53;
		const int EXT_SENS_DATA_11   =0x54;
		const int EXT_SENS_DATA_12   =0x55;
		const int EXT_SENS_DATA_13   =0x56;
		const int EXT_SENS_DATA_14   =0x57;
		const int EXT_SENS_DATA_15   =0x58;
		const int EXT_SENS_DATA_16   =0x59;
		const int EXT_SENS_DATA_17   =0x5A;
		const int EXT_SENS_DATA_18   =0x5B;
		const int EXT_SENS_DATA_19   =0x5C;
		const int EXT_SENS_DATA_20   =0x5D;
		const int EXT_SENS_DATA_21   =0x5E;
		const int EXT_SENS_DATA_22   =0x5F;
		const int EXT_SENS_DATA_23   =0x60;
		const int MOT_DETECT_STATUS  =0x61;
		const int I2C_SLV0_DO        =0x63;
		const int I2C_SLV1_DO        =0x64;
		const int I2C_SLV2_DO        =0x65;
		const int I2C_SLV3_DO        =0x66;
		const int I2C_MST_DELAY_CTRL =0x67;
		const int SIGNAL_PATH_RESET  =0x68;
		const int MOT_DETECT_CTRL    =0x69;
		const int USER_CTRL          =0x6A; // Bit 7 enable DMP, bit 3 reset DMP
		const int PWR_MGMT_1         =0x6B; // Device defaults to the SLEEP mode
		const int PWR_MGMT_2         =0x6C;
		const int DMP_BANK           =0x6D;  // Activates a specific bank in the DMP
		const int DMP_RW_PNT         =0x6E;  // Set read/write pointer to a specific start address in specified DMP bank
		const int DMP_REG            =0x6F;  // Register in DMP from which to read or to which to write
		const int DMP_REG_1          =0x70;
		const int DMP_REG_2          =0x71;
		const int FIFO_COUNTH        =0x72;
		const int FIFO_COUNTL        =0x73;
		const int FIFO_R_W           =0x74;
		const int WHO_AM_I_MPU9250   =0x75; // Should return 0x71
		const int XA_OFFSET_H        =0x77;
		const int XA_OFFSET_L        =0x78;
		const int YA_OFFSET_H        =0x7A;
		const int YA_OFFSET_L        =0x7B;
		const int ZA_OFFSET_H        =0x7D;
		const int ZA_OFFSET_L        =0x7E;

// Using the MPU-9250 breakout board, ADO is set to 0
// Seven-bit device address is 110100 for ADO = 0 and 110101 for ADO = 1
const int ADO = 0;
#if ADO
		const int MPU9250_ADDRESS	 =0x69  // Device address when ADO = 1
#else
		const int MPU9250_ADDRESS	 =0x68;  // Device address when ADO = 0
		const int AK8963_ADDRESS	 =0x0C;   // Address of magnetometer
#endif // AD0

		const int READ_FLAG			 =0x80;
		const int NOT_SPI			 =-1;
		const int SPI_DATA_RATE		 =1000000; // 1MHz is the max speed of the MPU-9250
//		const int SPI_DATA_RATE		 =1000000; // 1MHz is the max speed of the MPU-9250
//		const int SPI_MODE			 =SPI_MODE3;

		enum ASCALE {
			AFS_2G = 0,
			AFS_4G,
			AFS_8G,
			AFS_16G
		};

		enum GSCALE {
			GFS_250DPS = 0,
			GFS_500DPS,
			GFS_1000DPS,
			GFS_2000DPS
		};

		enum MSCALE {
			MFS_14BITS = 0, // 0.6 mG per LSB
			MFS_16BITS      // 0.15 mG per LSB
		};

		enum M_MODE {
			M_8HZ = 0x02,  // 8 Hz update
			M_100HZ = 0x06 // 100 Hz continuous magnetometer
		};

		// TODO: Add setter methods for this hard coded stuff
		// Specify sensor full scale
		byte Gscale = (byte)GSCALE.GFS_250DPS;
		byte Ascale = (byte)ASCALE.AFS_2G;
		// Choose either 14-bit or 16-bit magnetometer resolution
		byte Mscale = (byte)MSCALE.MFS_16BITS;

		// 2 for 8 Hz, 6 for 100 Hz continuous magnetometer data read
		byte Mmode = (byte)M_MODE.M_8HZ;

		// SPI chip select pin
		byte _csPin;

		//byte writeByteWire(byte, byte, byte);
		//byte writeByteSPI(byte, byte);
		//byte readByteSPI(byte subAddress);
		//byte readByteWire(byte address, byte subAddress);
		//bool magInit();
		//void kickHardware();
		//void select();
		//void deselect();
	// TODO: Remove this next line
	//public:
	//    byte ak8963WhoAmI_SPI();

	  //public:
		float	pitch, yaw, roll;
		float	temperature;   // Stores the real internal chip temperature in Celsius
		short	tempCount;   // Temperature raw count output
		UInt32	delt_t = 0; // Used to control display output rate
	
		UInt32 count = 0, sumCount = 0; // used to control display output rate
		float deltat = 0.0f, sum = 0.0f;  // integration interval for both filter schemes
		UInt32 lastUpdate = 0, firstUpdate = 0; // used to calculate integration interval
		UInt32 Now = 0;        // used to calculate integration interval

		Int16[] gyroCount = {0,0,0};   // Stores the 16-bit signed gyro sensor output
		UInt16[] magCount = {0,0,0};    // Stores the 16-bit signed magnetometer sensor output
		// Scale resolutions per LSB for the sensors
		float aRes, gRes, mRes;
		// Variables to hold latest sensor data values
		float ax, ay, az, gx, gy, gz, mx, my, mz;
		// Factory mag calibration and mag bias
		float[] factoryMagCalibration = {0, 0, 0};
		float[]	factoryMagBias = {0, 0, 0};
		// Bias corrections for gyro, accelerometer, and magnetometer
		float[]	gyroBias  = {0, 0, 0};
		float[]	accelBias = {0, 0, 0};
		float[] magBias   = {0, 0, 0};
		float[]	magScale  = {0, 0, 0};
		float[] selfTest = {0,0,0,0,0,0};
		// Stores the 16-bit signed accelerometer sensor output
		Int16[] accelCount ={0,0,0};

		// Public method declarations
		//MPU9250(int8_t csPin=NOT_SPI);
		//void getMres();
		//void getGres();
		//void getAres();
		//void readAccelData(int16_t *);
		//void readGyroData(int16_t *);
		//void readMagData(int16_t *);
		//int16_t readTempData();
		//void updateTime();
		//void initAK8963(float *);
		//void initMPU9250();
		//void calibrateMPU9250(float * gyroBias, float * accelBias);
		//void MPU9250SelfTest(float * destination);
		//void magCalMPU9250(float * dest1, float * dest2);
		//byte writeByte(byte, byte, byte);
		//byte readByte(byte, byte);
		//byte readBytes(byte, byte, byte, byte *);
		//// TODO: make SPI/Wire private
		//byte readBytesSPI(byte, byte, byte *);
		//byte readBytesWire(byte, byte, byte, byte *);
		//bool isInI2cMode() { return _csPin == -1; }
		//bool begin();


		//MPU9250(int8_t cspin /*=NOT_SPI*/) // Uses I2C communication by default
		//{
		//  // Use hardware SPI communication
		//  // If used with sparkfun breakout board
		//  // https://www.sparkfun.com/products/13762 , change the pre-soldered JP2 to
		//  // enable SPI (solder middle and left instead of middle and right) pads are
		//  // very small and re-soldering can be very tricky. I2C highly recommended.
		//  if ((cspin > NOT_SPI) && (cspin < NUM_DIGITAL_PINS))
		//  {
		//    _csPin = cspin;
		//    SPI.begin();
		//    pinMode(_csPin, OUTPUT);
		//    deselect();
		//  }
		//  else
		//  {
		//    _csPin = NOT_SPI;
		//    Wire.begin();
		//  }
		//}

		void getMres()
		{
			switch ((MSCALE)Mscale) {
			// Possible magnetometer scales (and their register bit settings) are:
			// 14 bit resolution (0) and 16 bit resolution (1)
			case MSCALE.MFS_14BITS:
				mRes = 10.0f * 4912.0f / 8190.0f; // Proper scale to return milliGauss
			break;
			case MSCALE.MFS_16BITS:
				mRes = 10.0f * 4912.0f / 32760.0f; // Proper scale to return milliGauss
			break;
			}
		}

		void getGres()
		{
			switch ((GSCALE)Gscale)
			{
			// Possible gyro scales (and their register bit settings) are:
			// 250 DPS (00), 500 DPS (01), 1000 DPS (10), and 2000 DPS (11).
			// Here's a bit of an algorith to calculate DPS/(ADC tick) based on that
			// 2-bit value:
			case GSCALE.GFS_250DPS:
				gRes = 250.0f / 32768.0f;
			break;
			case GSCALE.GFS_500DPS:
				gRes = 500.0f / 32768.0f;
			break;
			case GSCALE.GFS_1000DPS:
				gRes = 1000.0f / 32768.0f;
			break;
			case GSCALE.GFS_2000DPS:
				gRes = 2000.0f / 32768.0f;
			break;
			}
		}

		void getAres()
		{
			switch ((ASCALE)Ascale)
			{
			// Possible accelerometer scales (and their register bit settings) are:
			// 2 Gs (00), 4 Gs (01), 8 Gs (10), and 16 Gs  (11).
			// Here's a bit of an algorith to calculate DPS/(ADC tick) based on that
			// 2-bit value:
			case ASCALE.AFS_2G:
				aRes = 2.0f / 32768.0f;
				break;
			case ASCALE.AFS_4G:
				aRes = 4.0f / 32768.0f;
				break;
			case ASCALE.AFS_8G:
				aRes = 8.0f / 32768.0f;
				break;
			case ASCALE.AFS_16G:
				aRes = 16.0f / 32768.0f;
				break;
			}
		}


		void readAccelData(Int16[] destination)
		{
			byte[] rawData = new byte[6];  // x/y/z accel register data stored here
			// Read the six raw data registers into data array
			READ_BLK(ACCEL_XOUT_H, 6, ref rawData);

			// Turn the MSB and LSB into a signed 16-bit value
			destination[0] = (Int16)(((Int16)rawData[0] << 8) | rawData[1]);
			destination[1] = (Int16)(((Int16)rawData[2] << 8) | rawData[3]);
			destination[2] = (Int16)(((Int16)rawData[4] << 8) | rawData[5]);
		}


		void readGyroData(Int16[] destination)
		{
			byte[] rawData = new byte[6];  // x/y/z gyro register data stored here
			// Read the six raw data registers sequentially into data array
			READ_BLK(GYRO_XOUT_H, 6, ref rawData);

			// Turn the MSB and LSB into a signed 16-bit value
			destination[0] = (Int16)(((Int16)rawData[0] << 8) | rawData[1]);
			destination[1] = (Int16)(((Int16)rawData[2] << 8) | rawData[3]);
			destination[2] = (Int16)(((Int16)rawData[4] << 8) | rawData[5]);
		}

		//void readMagData(Int16[] destination)
		//{
		//    // x/y/z gyro register data, ST2 register stored here, must read ST2 at end
		//    // of data acquisition
		//    byte[]	rawData = new byte[7];
		//    byte	ret;

		//    // Wait for magnetometer data ready bit to be set
		//    ret = READ_ONE(AK8963_ST1);
		//    if (ret & 0x01) {
		//        // Read the six raw data and ST2 registers sequentially into data array
		//        readBytes(AK8963_ADDRESS, AK8963_XOUT_L, 7, &rawData[0]);
		//        uint8_t c = rawData[6]; // End data read by reading ST2 register
		//        // Check if magnetic sensor overflow set, if not then report data
		//        if (!(c & 0x08)) {
		//            // Turn the MSB and LSB into a signed 16-bit value
		//            destination[0] = ((int16_t)rawData[1] << 8) | rawData[0];
		//            // Data stored as little Endian
		//            destination[1] = ((int16_t)rawData[3] << 8) | rawData[2];
		//            destination[2] = ((int16_t)rawData[5] << 8) | rawData[4];
		//        }
		//    }
		//}

		Int16 readTempData()
		{
			byte[] rawData = new byte[2]; // x/y/z gyro register data stored here

			// Read the two raw data registers sequentially into data array
			READ_BLK(TEMP_OUT_H, 2, ref rawData);
			// Turn the MSB and LSB into a 16-bit value
			return (Int16)(((Int16)rawData[0] << 8) | rawData[1]);
		}

		// Calculate the time the last update took for use in the quaternion filters
		// TODO: This doesn't really belong in this class.
		//void updateTime()
		//{
		//    Now = micros();

		//    // Set integration time by time elapsed since last filter update
		//    deltat = ((Now - lastUpdate) / 1000000.0f);
		//    lastUpdate = Now;

		//    sum += deltat; // sum for averaging filter update rate
		//    sumCount++;
		//}

		//void initAK8963(float * destination)
		//{
		//  // First extract the factory calibration for each magnetometer axis
		//  uint8_t rawData[3];  // x/y/z gyro calibration data stored here
		//  // TODO: Test this!! Likely doesn't work
		//  writeByte(AK8963_ADDRESS, AK8963_CNTL, 0x00); // Power down magnetometer
		//  delay(10);
		//  writeByte(AK8963_ADDRESS, AK8963_CNTL, 0x0F); // Enter Fuse ROM access mode
		//  delay(10);

		//  // Read the x-, y-, and z-axis calibration values
		//  readBytes(AK8963_ADDRESS, AK8963_ASAX, 3, &rawData[0]);

		//  // Return x-axis sensitivity adjustment values, etc.
		//  destination[0] =  (float)(rawData[0] - 128)/256. + 1.;
		//  destination[1] =  (float)(rawData[1] - 128)/256. + 1.;
		//  destination[2] =  (float)(rawData[2] - 128)/256. + 1.;
		//  writeByte(AK8963_ADDRESS, AK8963_CNTL, 0x00); // Power down magnetometer
		//  delay(10);

		//  // Configure the magnetometer for continuous read and highest resolution.
		//  // Set Mscale bit 4 to 1 (0) to enable 16 (14) bit resolution in CNTL
		//  // register, and enable continuous mode data acquisition Mmode (bits [3:0]),
		//  // 0010 for 8 Hz and 0110 for 100 Hz sample rates.

		//  // Set magnetometer data resolution and sample ODR
		//  writeByte(AK8963_ADDRESS, AK8963_CNTL, Mscale << 4 | Mmode);
		//  delay(10);
		//}
		void delay(int ms)
		{
			System.Threading.Thread.Sleep(ms);
		}
		void initMPU9250()
		{

			// wake up device
			// Clear sleep mode bit (6), enable all sensors
			WRITE_ONE(PWR_MGMT_1, 0x00);
			delay(100); // Wait for all registers to reset

			// Get stable time source
			// Auto select clock source to be PLL gyroscope reference if ready else
			WRITE_ONE(PWR_MGMT_1, 0x01);
			delay(200);
			// Configure Gyro and Thermometer
			// Disable FSYNC and set thermometer and gyro bandwidth to 41 and 42 Hz,
			// respectively;
			// minimum delay time for this setting is 5.9 ms, which means sensor fusion
			// update rates cannot be higher than 1 / 0.0059 = 170 Hz
			// DLPF_CFG = bits 2:0 = 011; this limits the sample rate to 1000 Hz for both
			// With the MPU9250, it is possible to get gyro sample rates of 32 kHz (!),
			// 8 kHz, or 1 kHz
			WRITE_ONE(CONFIG, 0x03);

			// Set sample rate = gyroscope output rate/(1 + SMPLRT_DIV)
			// Use a 200 Hz rate; a rate consistent with the filter update rate
			// determined inset in CONFIG above.
			WRITE_ONE(SMPLRT_DIV, 0x04);

			// Set gyroscope full scale range
			// Range selects FS_SEL and AFS_SEL are 0 - 3, so 2-bit values are
			// left-shifted into positions 4:3

			// get current GYRO_CONFIG register value
			int c = READ_ONE(GYRO_CONFIG);
			// c = c & ~0xE0; // Clear self-test bits [7:5]
			c = c & ~0x02; // Clear Fchoice bits [1:0]
			c = c & ~0x18; // Clear AFS bits [4:3]
			c = c | Gscale << 3; // Set full scale range for the gyro
			// Set Fchoice for the gyro to 11 by writing its inverse to bits 1:0 of
			// GYRO_CONFIG
			// c =| 0x00;
			// Write new GYRO_CONFIG value to register
			WRITE_ONE(GYRO_CONFIG, c );

			// Set accelerometer full-scale range configuration
			// Get current ACCEL_CONFIG register value
			c = READ_ONE(ACCEL_CONFIG);
			// c = c & ~0xE0; // Clear self-test bits [7:5]
			c = c & ~0x18;  // Clear AFS bits [4:3]
			c = c | Ascale << 3; // Set full scale range for the accelerometer
			// Write new ACCEL_CONFIG register value
			WRITE_ONE(ACCEL_CONFIG, c);

			// Set accelerometer sample rate configuration
			// It is possible to get a 4 kHz sample rate from the accelerometer by
			// choosing 1 for accel_fchoice_b bit [3]; in this case the bandwidth is
			// 1.13 kHz
			// Get current ACCEL_CONFIG2 register value
			c = READ_ONE(ACCEL_CONFIG2);
			c = c & ~0x0F; // Clear accel_fchoice_b (bit 3) and A_DLPFG (bits [2:0])
			c = c | 0x03;  // Set accelerometer rate to 1 kHz and bandwidth to 41 Hz
			// Write new ACCEL_CONFIG2 register value
			WRITE_ONE(ACCEL_CONFIG2, c);
			// The accelerometer, gyro, and thermometer are set to 1 kHz sample rates,
			// but all these rates are further reduced by a factor of 5 to 200 Hz because
			// of the SMPLRT_DIV setting

			// Configure Interrupts and Bypass Enable
			// Set interrupt pin active high, push-pull, hold interrupt pin level HIGH
			// until interrupt cleared, clear on read of INT_STATUS, and enable
			// I2C_BYPASS_EN so additional chips can join the I2C bus and all can be
			// controlled by the Arduino as master.
#if true
			WRITE_ONE(INT_PIN_CFG, 0x20);//磁気センサ(AK8963)はアクセスしない
#else
			WRITE_ONE(INT_PIN_CFG, 0x22);
#endif

			// Enable data ready (bit 0) interrupt
			WRITE_ONE(INT_ENABLE, 0x01);
			delay(100);
		}


		// Function which accumulates gyro and accelerometer data after device
		// initialization. It calculates the average of the at-rest readings and then
		// loads the resulting offsets into accelerometer and gyro bias registers.
		void calibrateMPU9250(float[] gyroBias, float[] accelBias)
		{
			byte[] data = new byte[12]; // data array to hold accelerometer and gyro x, y, z, data
			UInt16 ii, packet_count, fifo_count;
			Int32[] gyro_bias  = {0, 0, 0};
			Int32[] accel_bias = {0, 0, 0};

			// reset device
			// Write a one to bit 7 reset bit; toggle reset device
			WRITE_ONE(PWR_MGMT_1, READ_FLAG);
			delay(100);

			// get stable time source; Auto select clock source to be PLL gyroscope
			// reference if ready else use the internal oscillator, bits 2:0 = 001
			WRITE_ONE(PWR_MGMT_1, 0x01);
			WRITE_ONE(PWR_MGMT_2, 0x00);
			delay(200);

			// Configure device for bias calculation
			// Disable all interrupts
			WRITE_ONE(INT_ENABLE, 0x00);
			// Disable FIFO
			WRITE_ONE(FIFO_EN, 0x00);
			// Turn on internal clock source
			WRITE_ONE(PWR_MGMT_1, 0x00);
			// Disable I2C master
			WRITE_ONE(I2C_MST_CTRL, 0x00);
			// Disable FIFO and I2C master modes
			WRITE_ONE(USER_CTRL, 0x00);
			// Reset FIFO and DMP
			WRITE_ONE(USER_CTRL, 0x0C);
			delay(15);

			// Configure MPU6050 gyro and accelerometer for bias calculation
			// Set low-pass filter to 188 Hz
			WRITE_ONE(CONFIG, 0x01);
			// Set sample rate to 1 kHz
			WRITE_ONE(SMPLRT_DIV, 0x00);
			// Set gyro full-scale to 250 degrees per second, maximum sensitivity
			WRITE_ONE(GYRO_CONFIG, 0x00);
			// Set accelerometer full-scale to 2 g, maximum sensitivity
			WRITE_ONE(ACCEL_CONFIG, 0x00);

			UInt16  gyrosensitivity  = 131;   // = 131 LSB/degrees/sec
			UInt16  accelsensitivity = 16384; // = 16384 LSB/g

			// Configure FIFO to capture accelerometer and gyro data for bias calculation
			WRITE_ONE(USER_CTRL, 0x40);  // Enable FIFO
			// Enable gyro and accelerometer sensors for FIFO  (max size 512 bytes in
			// MPU-9150)
			WRITE_ONE(FIFO_EN, 0x78);
			delay(40);  // accumulate 40 samples in 40 milliseconds = 480 bytes

			// At end of sample accumulation, turn off FIFO sensor read
			// Disable gyro and accelerometer sensors for FIFO
			WRITE_ONE(FIFO_EN, 0x00);
			// Read FIFO sample count
			READ_BLK(FIFO_COUNTH, 2, ref data);
			fifo_count = (UInt16)(((UInt16)data[0] << 8) | data[1]);
			// How many sets of full gyro and accelerometer data for averaging
			packet_count = (UInt16)(fifo_count/12);

			for (ii = 0; ii < packet_count; ii++)
			{
				Int16[] accel_temp = {0, 0, 0};
				Int16[] gyro_temp = {0, 0, 0};
				// Read data for averaging
				READ_BLK(FIFO_R_W, 12, ref data);
				// Form signed 16-bit integer for each sample in FIFO
				accel_temp[0] = (Int16) (((Int16)data[0] << 8) | data[1]  );
				accel_temp[1] = (Int16) (((Int16)data[2] << 8) | data[3]  );
				accel_temp[2] = (Int16) (((Int16)data[4] << 8) | data[5]  );
				gyro_temp[0]  = (Int16) (((Int16)data[6] << 8) | data[7]  );
				gyro_temp[1]  = (Int16) (((Int16)data[8] << 8) | data[9]  );
				gyro_temp[2]  = (Int16) (((Int16)data[10] << 8) | data[11]);

				// Sum individual signed 16-bit biases to get accumulated signed 32-bit
				// biases.
				accel_bias[0] += (Int32) accel_temp[0];
				accel_bias[1] += (Int32) accel_temp[1];
				accel_bias[2] += (Int32) accel_temp[2];
				gyro_bias[0]  += (Int32) gyro_temp[0];
				gyro_bias[1]  += (Int32) gyro_temp[1];
				gyro_bias[2]  += (Int32) gyro_temp[2];
			}
			// Sum individual signed 16-bit biases to get accumulated signed 32-bit biases
			accel_bias[0] /= (Int32) packet_count;
			accel_bias[1] /= (Int32) packet_count;
			accel_bias[2] /= (Int32) packet_count;
			gyro_bias[0]  /= (Int32) packet_count;
			gyro_bias[1]  /= (Int32) packet_count;
			gyro_bias[2]  /= (Int32) packet_count;

			// Sum individual signed 16-bit biases to get accumulated signed 32-bit biases
			if (accel_bias[2] > 0L)
			{
				accel_bias[2] -= (Int32) accelsensitivity;
			}
			else
			{
				accel_bias[2] += (Int32) accelsensitivity;
			}

			// Construct the gyro biases for push to the hardware gyro bias registers,
			// which are reset to zero upon device startup.
			// Divide by 4 to get 32.9 LSB per deg/s to conform to expected bias input
			// format.
			data[0] = (byte)((-gyro_bias[0]/4  >> 8) & 0xFF);
			// Biases are additive, so change sign on calculated average gyro biases
			data[1] = (byte)((-gyro_bias[0]/4)       & 0xFF);
			data[2] = (byte)((-gyro_bias[1]/4  >> 8) & 0xFF);
			data[3] = (byte)((-gyro_bias[1]/4)       & 0xFF);
			data[4] = (byte)((-gyro_bias[2]/4  >> 8) & 0xFF);
			data[5] = (byte)((-gyro_bias[2]/4)       & 0xFF);

			// Push gyro biases to hardware registers
			WRITE_ONE(XG_OFFSET_H, data[0]);
			WRITE_ONE(XG_OFFSET_L, data[1]);
			WRITE_ONE(YG_OFFSET_H, data[2]);
			WRITE_ONE(YG_OFFSET_L, data[3]);
			WRITE_ONE(ZG_OFFSET_H, data[4]);
			WRITE_ONE(ZG_OFFSET_L, data[5]);

			// Output scaled gyro biases for display in the main program
			gyroBias[0] = (float) gyro_bias[0]/(float) gyrosensitivity;
			gyroBias[1] = (float) gyro_bias[1]/(float) gyrosensitivity;
			gyroBias[2] = (float) gyro_bias[2]/(float) gyrosensitivity;

			// Construct the accelerometer biases for push to the hardware accelerometer
			// bias registers. These registers contain factory trim values which must be
			// added to the calculated accelerometer biases; on boot up these registers
			// will hold non-zero values. In addition, bit 0 of the lower byte must be
			// preserved since it is used for temperature compensation calculations.
			// Accelerometer bias registers expect bias input as 2048 LSB per g, so that
			// the accelerometer biases calculated above must be divided by 8.

			// A place to hold the factory accelerometer trim biases
			Int32[] accel_bias_reg = {0, 0, 0};
			// Read factory accelerometer trim values
			READ_BLK(XA_OFFSET_H, 2, ref data);
			accel_bias_reg[0] = (Int32) (((Int16)data[0] << 8) | data[1]);
			READ_BLK(YA_OFFSET_H, 2, ref data);
			accel_bias_reg[1] = (Int32) (((Int16)data[0] << 8) | data[1]);
			READ_BLK(ZA_OFFSET_H, 2, ref data);
			accel_bias_reg[2] = (Int32) (((Int16)data[0] << 8) | data[1]);

			// Define mask for temperature compensation bit 0 of lower byte of
			// accelerometer bias registers
			UInt32 mask = 1;
			// Define array to hold mask bit for each accelerometer bias axis
			byte[] mask_bit = {0, 0, 0};

			for (ii = 0; ii < 3; ii++)
			{
				// If temperature compensation bit is set, record that fact in mask_bit
				if ((accel_bias_reg[ii] & mask)!=0) {
					mask_bit[ii] = 0x01;
				}
			}

			// Construct total accelerometer bias, including calculated average
			// accelerometer bias from above
			// Subtract calculated averaged accelerometer bias scaled to 2048 LSB/g
			// (16 g full scale)
			accel_bias_reg[0] -= (accel_bias[0]/8);
			accel_bias_reg[1] -= (accel_bias[1]/8);
			accel_bias_reg[2] -= (accel_bias[2]/8);

			data[0] = (byte)((accel_bias_reg[0] >> 8) & 0xFF);
			data[1] = (byte)((accel_bias_reg[0])      & 0xFF);
			// preserve temperature compensation bit when writing back to accelerometer
			// bias registers
			data[1] = (byte)(data[1] | mask_bit[0]);
			data[2] = (byte)((accel_bias_reg[1] >> 8) & 0xFF);
			data[3] = (byte)((accel_bias_reg[1])      & 0xFF);
			// Preserve temperature compensation bit when writing back to accelerometer
			// bias registers
			data[3] = (byte)(data[3] | mask_bit[1]);
			data[4] = (byte)((accel_bias_reg[2] >> 8) & 0xFF);
			data[5] = (byte)((accel_bias_reg[2])      & 0xFF);
			// Preserve temperature compensation bit when writing back to accelerometer
			// bias registers
			data[5] = (byte)(data[5] | mask_bit[2]);

			// Apparently this is not working for the acceleration biases in the MPU-9250
			// Are we handling the temperature correction bit properly?
			// Push accelerometer biases to hardware registers
			WRITE_ONE(XA_OFFSET_H, data[0]);
			WRITE_ONE(XA_OFFSET_L, data[1]);
			WRITE_ONE(YA_OFFSET_H, data[2]);
			WRITE_ONE(YA_OFFSET_L, data[3]);
			WRITE_ONE(ZA_OFFSET_H, data[4]);
			WRITE_ONE(ZA_OFFSET_L, data[5]);

			// Output scaled accelerometer biases for display in the main program
			accelBias[0] = (float)accel_bias[0]/(float)accelsensitivity;
			accelBias[1] = (float)accel_bias[1]/(float)accelsensitivity;
			accelBias[2] = (float)accel_bias[2]/(float)accelsensitivity;
		}


		// Accelerometer and gyroscope self test; check calibration wrt factory settings
		// Should return percent deviation from factory trim values, +/- 14 or less
		// deviation is a pass.
		void MPU9250SelfTest(float[] destination)
		{
			byte[] rawData = {0, 0, 0, 0, 0, 0};
			byte[] selfTest = new byte[6];
			Int32[] gAvg = {0,0,0};
			Int32[] aAvg = {0,0,0};
			Int32[] aSTAvg = {0,0,0};
			Int32[] gSTAvg = {0,0,0};
			float[] factoryTrim = new float[6];
			byte FS = 0;

			// Set gyro sample rate to 1 kHz
			WRITE_ONE(SMPLRT_DIV, 0x00);
			// Set gyro sample rate to 1 kHz and DLPF to 92 Hz
			WRITE_ONE(CONFIG, 0x02);
			// Set full scale range for the gyro to 250 dps
			WRITE_ONE(GYRO_CONFIG, 1<<FS);
			// Set accelerometer rate to 1 kHz and bandwidth to 92 Hz
			WRITE_ONE(ACCEL_CONFIG2, 0x02);
			// Set full scale range for the accelerometer to 2 g
			WRITE_ONE(ACCEL_CONFIG, 1<<FS);

			// Get average current values of gyro and acclerometer
			for (int ii = 0; ii < 200; ii++)
			{
				//Serial.print("BHW::ii = ");
				//Serial.println(ii);
				// Read the six raw data registers into data array
				READ_BLK(ACCEL_XOUT_H, 6, ref rawData);
				// Turn the MSB and LSB into a signed 16-bit value
				aAvg[0] += (Int16)(((Int16)rawData[0] << 8) | rawData[1]) ;
				aAvg[1] += (Int16)(((Int16)rawData[2] << 8) | rawData[3]) ;
				aAvg[2] += (Int16)(((Int16)rawData[4] << 8) | rawData[5]) ;

				// Read the six raw data registers sequentially into data array
				READ_BLK(GYRO_XOUT_H, 6, ref rawData);
				// Turn the MSB and LSB into a signed 16-bit value
				gAvg[0] += (Int16)(((Int16)rawData[0] << 8) | rawData[1]) ;
				gAvg[1] += (Int16)(((Int16)rawData[2] << 8) | rawData[3]) ;
				gAvg[2] += (Int16)(((Int16)rawData[4] << 8) | rawData[5]) ;
			}

			// Get average of 200 values and store as average current readings
			for (int ii =0; ii < 3; ii++)
			{
				aAvg[ii] /= 200;
				gAvg[ii] /= 200;
			}

			// Configure the accelerometer for self-test
			// Enable self test on all three axes and set accelerometer range to +/- 2 g
			WRITE_ONE(ACCEL_CONFIG, 0xE0);
			// Enable self test on all three axes and set gyro range to +/- 250 degrees/s
			WRITE_ONE(GYRO_CONFIG,  0xE0);
			delay(25);  // Delay a while to let the device stabilize

			// Get average self-test values of gyro and acclerometer
			for (int ii = 0; ii < 200; ii++)
			{
				// Read the six raw data registers into data array
				READ_BLK(ACCEL_XOUT_H, 6, ref rawData);
				// Turn the MSB and LSB into a signed 16-bit value
				aSTAvg[0] += (Int16)(((Int16)rawData[0] << 8) | rawData[1]) ;
				aSTAvg[1] += (Int16)(((Int16)rawData[2] << 8) | rawData[3]) ;
				aSTAvg[2] += (Int16)(((Int16)rawData[4] << 8) | rawData[5]) ;

				// Read the six raw data registers sequentially into data array
				READ_BLK(GYRO_XOUT_H, 6, ref rawData);
				// Turn the MSB and LSB into a signed 16-bit value
				gSTAvg[0] += (Int16)(((Int16)rawData[0] << 8) | rawData[1]) ;
				gSTAvg[1] += (Int16)(((Int16)rawData[2] << 8) | rawData[3]) ;
				gSTAvg[2] += (Int16)(((Int16)rawData[4] << 8) | rawData[5]) ;
			}

			// Get average of 200 values and store as average self-test readings
			for (int ii =0; ii < 3; ii++)
			{
				aSTAvg[ii] /= 200;
				gSTAvg[ii] /= 200;
			}

			// Configure the gyro and accelerometer for normal operation
			WRITE_ONE(ACCEL_CONFIG, 0x00);
			WRITE_ONE(GYRO_CONFIG,  0x00);
			delay(25);  // Delay a while to let the device stabilize

			// Retrieve accelerometer and gyro factory Self-Test Code from USR_Reg
			// X-axis accel self-test results
			selfTest[0] = (byte)READ_ONE(SELF_TEST_X_ACCEL);
			// Y-axis accel self-test results
			selfTest[1] = (byte)READ_ONE(SELF_TEST_Y_ACCEL);
			// Z-axis accel self-test results
			selfTest[2] = (byte)READ_ONE(SELF_TEST_Z_ACCEL);
			// X-axis gyro self-test results
			selfTest[3] = (byte)READ_ONE(SELF_TEST_X_GYRO);
			// Y-axis gyro self-test results
			selfTest[4] = (byte)READ_ONE(SELF_TEST_Y_GYRO);
			// Z-axis gyro self-test results
			selfTest[5] = (byte)READ_ONE(SELF_TEST_Z_GYRO);

			// Retrieve factory self-test value from self-test code reads
			// FT[Xa] factory trim calculation
			factoryTrim[0] = (float)((2620/1<<FS)*(Math.Pow(1.01 ,((float)selfTest[0] - 1.0) )));
			// FT[Ya] factory trim calculation
			factoryTrim[1] = (float)((2620/1<<FS)*(Math.Pow(1.01 ,((float)selfTest[1] - 1.0) )));
			// FT[Za] factory trim calculation
			factoryTrim[2] = (float)((2620/1<<FS)*(Math.Pow(1.01 ,((float)selfTest[2] - 1.0) )));
			// FT[Xg] factory trim calculation
			factoryTrim[3] = (float)((2620/1<<FS)*(Math.Pow(1.01 ,((float)selfTest[3] - 1.0) )));
			// FT[Yg] factory trim calculation
			factoryTrim[4] = (float)((2620/1<<FS)*(Math.Pow(1.01 ,((float)selfTest[4] - 1.0) )));
			// FT[Zg] factory trim calculation
			factoryTrim[5] = (float)((2620/1<<FS)*(Math.Pow(1.01 ,((float)selfTest[5] - 1.0) )));

			// Report results as a ratio of (STR - FT)/FT; the change from Factory Trim
			// of the Self-Test Response
			// To get percent, must multiply by 100
			for (int i = 0; i < 3; i++)
			{
				// Report percent differences
				destination[i+0] = (float)(100.0 * ((float)(aSTAvg[i] - aAvg[i])) / factoryTrim[i]- 100.0);
				// Report percent differences
				destination[i+3] = (float)(100.0*((float)(gSTAvg[i] - gAvg[i]))/factoryTrim[i+3]- 100.0);
			}
		}

		//// Function which accumulates magnetometer data after device initialization.
		//// It calculates the bias and scale in the x, y, and z axes.
		//void magCalMPU9250(float * bias_dest, float * scale_dest)
		//{
		//  UInt16 ii = 0, sample_count = 0;
		//  Int32 mag_bias[3]  = {0, 0, 0},
		//          mag_scale[3] = {0, 0, 0};
		//  Int16 mag_max[3]  = {0x8000, 0x8000, 0x8000},
		//          mag_min[3]  = {0x7FFF, 0x7FFF, 0x7FFF},
		//          mag_temp[3] = {0, 0, 0};

		//  // Make sure resolution has been calculated
		//  getMres();

		//  Serial.println(F("Mag Calibration: Wave device in a figure 8 until done!"));
		//  Serial.println(
		//      F("  4 seconds to get ready followed by 15 seconds of sampling)"));
		//  delay(4000);

		//  // shoot for ~fifteen seconds of mag data
		//  // at 8 Hz ODR, new mag data is available every 125 ms
		//  if (Mmode == M_8HZ)
		//  {
		//    sample_count = 128;
		//  }
		//  // at 100 Hz ODR, new mag data is available every 10 ms
		//  if (Mmode == M_100HZ)
		//  {
		//    sample_count = 1500;
		//  }

		//  for (ii = 0; ii < sample_count; ii++)
		//  {
		//    readMagData(mag_temp);  // Read the mag data

		//    for (int jj = 0; jj < 3; jj++)
		//    {
		//      if (mag_temp[jj] > mag_max[jj])
		//      {
		//        mag_max[jj] = mag_temp[jj];
		//      }
		//      if (mag_temp[jj] < mag_min[jj])
		//      {
		//        mag_min[jj] = mag_temp[jj];
		//      }
		//    }

		//    if (Mmode == M_8HZ)
		//    {
		//      delay(135); // At 8 Hz ODR, new mag data is available every 125 ms
		//    }
		//    if (Mmode == M_100HZ)
		//    {
		//      delay(12);  // At 100 Hz ODR, new mag data is available every 10 ms
		//    }
		//  }

		//  // Serial.println("mag x min/max:"); Serial.println(mag_max[0]); Serial.println(mag_min[0]);
		//  // Serial.println("mag y min/max:"); Serial.println(mag_max[1]); Serial.println(mag_min[1]);
		//  // Serial.println("mag z min/max:"); Serial.println(mag_max[2]); Serial.println(mag_min[2]);

		//  // Get hard iron correction
		//  // Get 'average' x mag bias in counts
		//  mag_bias[0]  = (mag_max[0] + mag_min[0]) / 2;
		//  // Get 'average' y mag bias in counts
		//  mag_bias[1]  = (mag_max[1] + mag_min[1]) / 2;
		//  // Get 'average' z mag bias in counts
		//  mag_bias[2]  = (mag_max[2] + mag_min[2]) / 2;

		//  // Save mag biases in G for main program
		//  bias_dest[0] = (float)mag_bias[0] * mRes * factoryMagCalibration[0];
		//  bias_dest[1] = (float)mag_bias[1] * mRes * factoryMagCalibration[1];
		//  bias_dest[2] = (float)mag_bias[2] * mRes * factoryMagCalibration[2];

		//  // Get soft iron correction estimate
		//  // Get average x axis max chord length in counts
		//  mag_scale[0]  = (mag_max[0] - mag_min[0]) / 2;
		//  // Get average y axis max chord length in counts
		//  mag_scale[1]  = (mag_max[1] - mag_min[1]) / 2;
		//  // Get average z axis max chord length in counts
		//  mag_scale[2]  = (mag_max[2] - mag_min[2]) / 2;

		//  float avg_rad = mag_scale[0] + mag_scale[1] + mag_scale[2];
		//  avg_rad /= 3.0;

		//  scale_dest[0] = avg_rad / ((float)mag_scale[0]);
		//  scale_dest[1] = avg_rad / ((float)mag_scale[1]);
		//  scale_dest[2] = avg_rad / ((float)mag_scale[2]);

		//  Serial.println(F("Mag Calibration done!"));
		//}

		// Select slave IC by asserting CS pin
		//void select()
		//{
		//  digitalWrite(_csPin, LOW);
		//}

		// Select slave IC by deasserting CS pin
		//void deselect()
		//{
		//  digitalWrite(_csPin, HIGH);
		//}

		//bool magInit()
		//{
		//  // Reset registers to defaults, bit auto clears
		//  writeByteSPI(0x6B, 0x80);
		//  // Auto select the best available clock source
		//  writeByteSPI(0x6B, 0x01);
		//  // Enable X,Y, & Z axes of accel and gyro
		//  writeByteSPI(0x6C, 0x00);
		//  // Config disable FSYNC pin, set gyro/temp bandwidth to 184/188 Hz
		//  writeByteSPI(0x1A, 0x01);
		//  // Self tests off, gyro set to +/-2000 dps FS
		//  writeByteSPI(0x1B, 0x18);
		//  // Self test off, accel set to +/- 8g FS
		//  writeByteSPI(0x1C, 0x08);
		//  // Bypass DLPF and set accel bandwidth to 184 Hz
		//  writeByteSPI(0x1D, 0x09);
		//  // Configure INT pin (active high / push-pull / latch until read)
		//  writeByteSPI(0x37, 0x30);
		//  // Enable I2C master mode
		//  // TODO Why not do this 11-100 ms after power up?
		//  writeByteSPI(0x6A, 0x20);
		//  // Disable multi-master and set I2C master clock to 400 kHz
		//  //https://developer.mbed.org/users/kylongmu/code/MPU9250_SPI/ calls says
		//  // enabled multi-master... TODO Find out why
		//  writeByteSPI(0x24, 0x0D);
		//  // Set to write to slave address 0x0C
		//  writeByteSPI(0x25, 0x0C);
		//  // Point save 0 register at AK8963's control 2 (soft reset) register
		//  writeByteSPI(0x26, 0x0B);
		//  // Send 0x01 to AK8963 via slave 0 to trigger a soft restart
		//  writeByteSPI(0x63, 0x01);
		//  // Enable simple 1-byte I2C reads from slave 0
		//  writeByteSPI(0x27, 0x81);
		//  // Point save 0 register at AK8963's control 1 (mode) register
		//  writeByteSPI(0x26, 0x0A);
		//  // 16-bit continuous measurement mode 1
		//  writeByteSPI(0x63, 0x12);
		//  // Enable simple 1-byte I2C reads from slave 0
		//  writeByteSPI(0x27, 0x81);

		//  // TODO: Remove this code
		//  uint8_t ret = ak8963WhoAmI_SPI();
		//#ifdef SERIAL_DEBUG
		//  Serial.print("magInit to return ");
		//  Serial.println((ret == 0x48) ? "true" : "false");
		//#endif
		//  return ret == 0x48;
		//}

		//// Read the WHOAMI (WIA) register of the AK8963
		//// TODO: This method has side effects
		//uint8_t ak8963WhoAmI_SPI()
		//{
		//  uint8_t response, oldSlaveAddress, oldSlaveRegister, oldSlaveConfig;
		//  // Save state
		//  oldSlaveAddress  = readByteSPI(I2C_SLV0_ADDR);
		//  oldSlaveRegister = readByteSPI(I2C_SLV0_REG);
		//  oldSlaveConfig   = readByteSPI(I2C_SLV0_CTRL);
		//#ifdef SERIAL_DEBUG
		//  Serial.print("Old slave address: 0x");
		//  Serial.println(oldSlaveAddress, HEX);
		//  Serial.print("Old slave register: 0x");
		//  Serial.println(oldSlaveRegister, HEX);
		//  Serial.print("Old slave config: 0x");
		//  Serial.println(oldSlaveConfig, HEX);
		//#endif

		//  // Set the I2C slave addres of AK8963 and set for read
		//  response = writeByteSPI(I2C_SLV0_ADDR, AK8963_ADDRESS|READ_FLAG);
		//  // I2C slave 0 register address from where to begin data transfer
		//  response = writeByteSPI(I2C_SLV0_REG, 0x00);
		//  // Enable 1-byte reads on slave 0
		//  response = writeByteSPI(I2C_SLV0_CTRL, 0x81);
		//  delayMicroseconds(1);
		//  // Read WIA register
		//  response = writeByteSPI(WHO_AM_I_AK8963|READ_FLAG, 0x00);

		//  // Restore state
		//  writeByteSPI(I2C_SLV0_ADDR, oldSlaveAddress);
		//  writeByteSPI(I2C_SLV0_REG, oldSlaveRegister);
		//  writeByteSPI(I2C_SLV0_CTRL, oldSlaveConfig);

		//  return response;
		//}
		//---
		bool setup()
		{
			//  Wire.begin();
			//  TWBR = 12;  // 400 kbit/sec I2C speed
			// Setup for Master mode, pins 18/19, external pullups, 400kHz
			//Wire.begin(I2C_MASTER, 0x00, I2C_PINS_16_17, I2C_PULLUP_EXT, I2C_RATE_100);
			//Serial.begin(38400);
  
			// Set up the interrupt pin, its set as active high, push-pull
			//pinMode(intPin, INPUT);
			//digitalWrite(intPin, LOW);
			//pinMode(adoPin, OUTPUT);
			//digitalWrite(adoPin, HIGH);
			//pinMode(myLed, OUTPUT);
			//digitalWrite(myLed, HIGH);
  
			//display.begin(); // Initialize the display
			//display.setContrast(58); // Set the contrast
  
			// Start device display with ID of sensor
			//display.clearDisplay();
			//display.setTextSize(2);
			//display.setCursor(0,0); display.print("MPU9250");
			//display.setTextSize(1);
			//display.setCursor(0, 20); display.print("9-DOF 16-bit");
			//display.setCursor(0, 30); display.print("motion sensor");
			//display.setCursor(20,40); display.print("60 ug LSB");
			//display.display();
			//delay(1000);

			// Set up for data display
			//display.setTextSize(1); // Set text size to normal, 2 is twice normal etc.
			//display.setTextColor(BLACK); // Set pixel color; 1 on the monochrome screen
			//display.clearDisplay();   // clears the screen and buffer

			// Read the WHO_AM_I register, this is a good test of communication
			int c = READ_ONE(WHO_AM_I_MPU9250);  // Read WHO_AM_I register for MPU-9250
			//Serial.print("MPU9250 "); Serial.print("I AM "); Serial.print(c, HEX); Serial.print(" I should be "); Serial.println(0x71, HEX);
			//display.setCursor(20,0); display.print("MPU9250");
			//display.setCursor(0,10); display.print("I AM");
			//display.setCursor(0,20); display.print(c, HEX);  
			//display.setCursor(0,30); display.print("I Should Be");
			//display.setCursor(0,40); display.print(0x71, HEX); 
			//display.display();
			//delay(5000); 

			if (c == 0x71) // WHO_AM_I should always be 0x68
			{  
				//Serial.println("MPU9250 is online...");
    
				MPU9250SelfTest(selfTest); // Start by performing self test and reporting values
				//Serial.print("x-axis self test: acceleration trim within : "); Serial.print(SelfTest[0],1); Serial.println("% of factory value");
				//Serial.print("y-axis self test: acceleration trim within : "); Serial.print(SelfTest[1],1); Serial.println("% of factory value");
				//Serial.print("z-axis self test: acceleration trim within : "); Serial.print(SelfTest[2],1); Serial.println("% of factory value");
				//Serial.print("x-axis self test: gyration trim within : "); Serial.print(SelfTest[3],1); Serial.println("% of factory value");
				//Serial.print("y-axis self test: gyration trim within : "); Serial.print(SelfTest[4],1); Serial.println("% of factory value");
				//Serial.print("z-axis self test: gyration trim within : "); Serial.print(SelfTest[5],1); Serial.println("% of factory value");
				//delay(5000);
    
				calibrateMPU9250(gyroBias, accelBias); // Calibrate gyro and accelerometers, load biases in bias registers
				//display.clearDisplay();
     
				//display.setCursor(0, 0); display.print("MPU9250 bias");
				//display.setCursor(0, 8); display.print(" x   y   z  ");

				//display.setCursor(0,  16); display.print((int)(1000*accelBias[0])); 
				//display.setCursor(24, 16); display.print((int)(1000*accelBias[1])); 
				//display.setCursor(48, 16); display.print((int)(1000*accelBias[2])); 
				//display.setCursor(72, 16); display.print("mg");
    
				//display.setCursor(0,  24); display.print(gyroBias[0], 1); 
				//display.setCursor(24, 24); display.print(gyroBias[1], 1); 
				//display.setCursor(48, 24); display.print(gyroBias[2], 1); 
				//display.setCursor(66, 24); display.print("o/s");   
 
				//display.display();
				//delay(1000); 
  
				initMPU9250(); 
				//Serial.println("MPU9250 initialized for active data mode...."); // Initialize device for active mode read of acclerometer, gyroscope, and temperature
  
				// Read the WHO_AM_I register of the magnetometer, this is a good test of communication
				//byte d = readByte(AK8963_ADDRESS, AK8963_WHO_AM_I);  // Read WHO_AM_I register for AK8963
				//Serial.print("AK8963 "); Serial.print("I AM "); Serial.print(d, HEX); Serial.print(" I should be "); Serial.println(0x48, HEX);
				//display.clearDisplay();
				//display.setCursor(20,0); display.print("AK8963");
				//display.setCursor(0,10); display.print("I AM");
				//display.setCursor(0,20); display.print(d, HEX);  
				//display.setCursor(0,30); display.print("I Should Be");
				//display.setCursor(0,40); display.print(0x48, HEX);  
				//display.display();
				//delay(1000); 
  
				// Get magnetometer calibration from AK8963 ROM
				//initAK8963(magCalibration); Serial.println("AK8963 initialized for active data mode...."); // Initialize device for active mode read of magnetometer
  
				//if(SerialDebug) {
				//    //  Serial.println("Calibration values: ");
				//    Serial.print("X-Axis sensitivity adjustment value "); Serial.println(magCalibration[0], 2);
				//    Serial.print("Y-Axis sensitivity adjustment value "); Serial.println(magCalibration[1], 2);
				//    Serial.print("Z-Axis sensitivity adjustment value "); Serial.println(magCalibration[2], 2);
				//}
  
				//display.clearDisplay();
				//display.setCursor(20,0); display.print("AK8963");
				//display.setCursor(0,10); display.print("ASAX "); display.setCursor(50,10); display.print(magCalibration[0], 2);
				//display.setCursor(0,20); display.print("ASAY "); display.setCursor(50,20); display.print(magCalibration[1], 2);
				//display.setCursor(0,30); display.print("ASAZ "); display.setCursor(50,30); display.print(magCalibration[2], 2);
				//display.display();
				//delay(1000);  
			}
			else {
				//Serial.print("Could not connect to MPU9250: 0x");
				//Serial.println(c, HEX);
				//while(1) ; // Loop forever if communication doesn't happen
				return(false);
			}
			return(true);
		}
		void loop()
		{
			int ret;
			// If intPin goes high, all data registers have new data
			ret = READ_ONE(INT_STATUS);
			if ((ret & 0x01)!=0) {  // On interrupt, check if data ready interrupt
				readAccelData(accelCount);  // Read the x/y/z adc values
				getAres();
    
				// Now we'll calculate the accleration value into actual g's
				ax = (float)accelCount[0]*aRes; // - accelBias[0];  // get actual g value, this depends on scale being set
				ay = (float)accelCount[1]*aRes; // - accelBias[1];   
				az = (float)accelCount[2]*aRes; // - accelBias[2];  
   
				readGyroData(gyroCount);  // Read the x/y/z adc values
				getGres();
 
				// Calculate the gyro value into actual degrees per second
				gx = (float)gyroCount[0]*gRes;  // get actual gyro value, this depends on scale being set
				gy = (float)gyroCount[1]*gRes;  
				gz = (float)gyroCount[2]*gRes;   
  
				//readMagData(magCount);  // Read the x/y/z adc values
				//getMres();
				//magbias[0] = +470.;  // User environmental x-axis correction in milliGauss, should be automatically calculated
				//magbias[1] = +120.;  // User environmental x-axis correction in milliGauss
				//magbias[2] = +125.;  // User environmental x-axis correction in milliGauss
    
				// Calculate the magnetometer values in milliGauss
				// Include factory calibration per data sheet and user environmental corrections
				//mx = (float)magCount[0]*mRes*magCalibration[0] - magbias[0];  // get actual magnetometer value, this depends on scale being set
				//my = (float)magCount[1]*mRes*magCalibration[1] - magbias[1];  
				//mz = (float)magCount[2]*mRes*magCalibration[2] - magbias[2];   
			}
			else {
				ret = ret;
			}
			//Now = micros();
			//deltat = ((Now - lastUpdate)/1000000.0f); // set integration time by time elapsed since last filter update
			//lastUpdate = Now;

			//sum += deltat; // sum for averaging filter update rate
			//sumCount++;
  
			// Sensors x (y)-axis of the accelerometer is aligned with the y (x)-axis of the magnetometer;
			// the magnetometer z-axis (+ down) is opposite to z-axis (+ up) of accelerometer and gyro!
			// We have to make some allowance for this orientationmismatch in feeding the output to the quaternion filter.
			// For the MPU-9250, we have chosen a magnetic rotation that keeps the sensor forward along the x-axis just like
			// in the LSM9DS0 sensor. This rotation can be modified to allow any convenient orientation convention.
			// This is ok by aircraft orientation standards!  
			// Pass gyro rate as rad/s
			//MadgwickQuaternionUpdate(ax, ay, az, gx*PI/180.0f, gy*PI/180.0f, gz*PI/180.0f,  my,  mx, mz);
		//  MahonyQuaternionUpdate(ax, ay, az, gx*PI/180.0f, gy*PI/180.0f, gz*PI/180.0f, my, mx, mz);


			if (AHRS == 0) {
				//delt_t = millis() - count;
				if (true/*delt_t > 500*/) {

					//if(SerialDebug) {
					//    // Print acceleration values in milligs!
					//    Serial.print("X-acceleration: "); Serial.print(1000*ax); Serial.print(" mg ");
					//    Serial.print("Y-acceleration: "); Serial.print(1000*ay); Serial.print(" mg ");
					//    Serial.print("Z-acceleration: "); Serial.print(1000*az); Serial.println(" mg ");
 
					//    // Print gyro values in degree/sec
					//    Serial.print("X-gyro rate: "); Serial.print(gx, 3); Serial.print(" degrees/sec "); 
					//    Serial.print("Y-gyro rate: "); Serial.print(gy, 3); Serial.print(" degrees/sec "); 
					//    Serial.print("Z-gyro rate: "); Serial.print(gz, 3); Serial.println(" degrees/sec"); 
    
					//    // Print mag values in degree/sec
					//    Serial.print("X-mag field: "); Serial.print(mx); Serial.print(" mG "); 
					//    Serial.print("Y-mag field: "); Serial.print(my); Serial.print(" mG "); 
					//    Serial.print("Z-mag field: "); Serial.print(mz); Serial.println(" mG"); 
 
					//    tempCount = readTempData();  // Read the adc values
					//    temperature = ((float) tempCount) / 333.87 + 21.0; // Temperature in degrees Centigrade
					//   // Print temperature in degrees Centigrade      
					//    Serial.print("Temperature is ");  Serial.print(temperature, 1);  Serial.println(" degrees C"); // Print T values to tenths of s degree C
					//}
   
					//display.clearDisplay();     
					//display.setCursor(0, 0); display.print("MPU9250/AK8963");
					//display.setCursor(0, 8); display.print(" x   y   z  ");

					//display.setCursor(0,  16); display.print((int)(1000*ax)); 
					//display.setCursor(24, 16); display.print((int)(1000*ay)); 
					//display.setCursor(48, 16); display.print((int)(1000*az)); 
					//display.setCursor(72, 16); display.print("mg");
    
					//display.setCursor(0,  24); display.print((int)(gx)); 
					//display.setCursor(24, 24); display.print((int)(gy)); 
					//display.setCursor(48, 24); display.print((int)(gz)); 
					//display.setCursor(66, 24); display.print("o/s");    
        
					//display.setCursor(0,  32); display.print((int)(mx)); 
					//display.setCursor(24, 32); display.print((int)(my)); 
					//display.setCursor(48, 32); display.print((int)(mz)); 
					//display.setCursor(72, 32); display.print("mG");   
   
					//display.setCursor(0,  40); display.print("Gyro T "); 
					//display.setCursor(50,  40); display.print(temperature, 1); display.print(" C");
					//display.display();
    
					//count = millis();
				}
			}
			else {
				// Serial print and/or display at 0.5 s rate independent of data rates
				//delt_t = millis() - count;
				//if (delt_t > 500) { // update LCD once per half-second independent of read rate

				//    if(SerialDebug) {
				//        Serial.print("ax = "); Serial.print((int)1000*ax);  
				//        Serial.print(" ay = "); Serial.print((int)1000*ay); 
				//        Serial.print(" az = "); Serial.print((int)1000*az); Serial.println(" mg");
				//        Serial.print("gx = "); Serial.print( gx, 2); 
				//        Serial.print(" gy = "); Serial.print( gy, 2); 
				//        Serial.print(" gz = "); Serial.print( gz, 2); Serial.println(" deg/s");
				//        Serial.print("mx = "); Serial.print( (int)mx ); 
				//        Serial.print(" my = "); Serial.print( (int)my ); 
				//        Serial.print(" mz = "); Serial.print( (int)mz ); Serial.println(" mG");
    
				//        Serial.print("q0 = "); Serial.print(q[0]);
				//        Serial.print(" qx = "); Serial.print(q[1]); 
				//        Serial.print(" qy = "); Serial.print(q[2]); 
				//        Serial.print(" qz = "); Serial.println(q[3]); 
				//    }               
   
				//    // Define output variables from updated quaternion---these are Tait-Bryan angles, commonly used in aircraft orientation.
				//    // In this coordinate system, the positive z-axis is down toward Earth. 
				//    // Yaw is the angle between Sensor x-axis and Earth magnetic North (or true North if corrected for local declination, looking down on the sensor positive yaw is counterclockwise.
				//    // Pitch is angle between sensor x-axis and Earth ground plane, toward the Earth is positive, up toward the sky is negative.
				//    // Roll is angle between sensor y-axis and Earth ground plane, y-axis up is positive roll.
				//    // These arise from the definition of the homogeneous rotation matrix constructed from quaternions.
				//    // Tait-Bryan angles as well as Euler angles are non-commutative; that is, the get the correct orientation the rotations must be
				//    // applied in the correct order which for this configuration is yaw, pitch, and then roll.
				//    // For more see http://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles which has additional links.
				//    yaw   = atan2(2.0f * (q[1] * q[2] + q[0] * q[3]), q[0] * q[0] + q[1] * q[1] - q[2] * q[2] - q[3] * q[3]);   
				//    pitch = -asin(2.0f * (q[1] * q[3] - q[0] * q[2]));
				//    roll  = atan2(2.0f * (q[0] * q[1] + q[2] * q[3]), q[0] * q[0] - q[1] * q[1] - q[2] * q[2] + q[3] * q[3]);
				//    pitch *= 180.0f / PI;
				//    yaw   *= 180.0f / PI; 
				//    yaw   -= 13.8; // Declination at Danville, California is 13 degrees 48 minutes and 47 seconds on 2014-04-04
				//    roll  *= 180.0f / PI;
     
				//    if(SerialDebug) {
				//        Serial.print("Yaw, Pitch, Roll: ");
				//        Serial.print(yaw, 2);
				//        Serial.print(", ");
				//        Serial.print(pitch, 2);
				//        Serial.print(", ");
				//        Serial.println(roll, 2);
    
				//        Serial.print("rate = "); Serial.print((float)sumCount/sum, 2); Serial.println(" Hz");
				//    }
   
				//    display.clearDisplay();    
 
				//    display.setCursor(0, 0); display.print(" x   y   z  ");

				//    display.setCursor(0,  8); display.print((int)(1000*ax)); 
				//    display.setCursor(24, 8); display.print((int)(1000*ay)); 
				//    display.setCursor(48, 8); display.print((int)(1000*az)); 
				//    display.setCursor(72, 8); display.print("mg");
    
				//    display.setCursor(0,  16); display.print((int)(gx)); 
				//    display.setCursor(24, 16); display.print((int)(gy)); 
				//    display.setCursor(48, 16); display.print((int)(gz)); 
				//    display.setCursor(66, 16); display.print("o/s");    

				//    display.setCursor(0,  24); display.print((int)(mx)); 
				//    display.setCursor(24, 24); display.print((int)(my)); 
				//    display.setCursor(48, 24); display.print((int)(mz)); 
				//    display.setCursor(72, 24); display.print("mG");    
 
				//    display.setCursor(0,  32); display.print((int)(yaw)); 
				//    display.setCursor(24, 32); display.print((int)(pitch)); 
				//    display.setCursor(48, 32); display.print((int)(roll)); 
				//    display.setCursor(66, 32); display.print("ypr");  
  
				//    // With these settings the filter is updating at a ~145 Hz rate using the Madgwick scheme and 
				//    // >200 Hz using the Mahony scheme even though the display refreshes at only 2 Hz.
				//    // The filter update rate is determined mostly by the mathematical steps in the respective algorithms, 
				//    // the processor speed (8 MHz for the 3.3V Pro Mini), and the magnetometer ODR:
				//    // an ODR of 10 Hz for the magnetometer produce the above rates, maximum magnetometer ODR of 100 Hz produces
				//    // filter update rates of 36 - 145 and ~38 Hz for the Madgwick and Mahony schemes, respectively. 
				//    // This is presumably because the magnetometer read takes longer than the gyro or accelerometer reads.
				//    // This filter update rate should be fast enough to maintain accurate platform orientation for 
				//    // stabilization control of a fast-moving robot or quadcopter. Compare to the update rate of 200 Hz
				//    // produced by the on-board Digital Motion Processor of Invensense's MPU6050 6 DoF and MPU9150 9DoF sensors.
				//    // The 3.3 V 8 MHz Pro Mini is doing pretty well!
				//    display.setCursor(0, 40); display.print("rt: "); display.print((float) sumCount / sum, 2); display.print(" Hz"); 
				//    display.display();

				//    digitalWrite(myLed, !digitalRead(myLed));
				//    count = millis(); 
				//    sumCount = 0;
				//    sum = 0;
				//}
			}
		}
		//---
		public void SET_BUS(int busadr)
		{
			this.busadr = busadr;
		}
		public override bool INIT()
		{
			setup();
			return(false);
		}
		public bool GET(double[] accl, double[] gyro)
		{
			loop();
			accl[0] = ax;
			accl[1] = ay;
			accl[2] = az;
			//---
			gyro[0] = gx;
			gyro[1] = gy;
			gyro[2] = gz;
			//---
			return(true);
		}
		override public bool GET(out object[] obj)
		{
			double[]	accl = {0,0,0},
						gyro = {0,0,0};
			obj = new object[6];
			GET(accl, gyro);
			obj[0] = accl[0];
			obj[1] = accl[1];
			obj[2] = accl[2];
			obj[3] = gyro[0];
			obj[4] = gyro[1];
			obj[5] = gyro[2];
			return(true);
		}
		override public int SENS_COUNT()
		{
			return(6);
		}
		override public string SENS_NAME(int idx)
		{
			string[] name = {
				"ACCEL.X", "ACCEL.Y", "ACCEL.Z",
				"GYRO.X", "GYRO.Y", "GYRO.Z"
			};
			if (idx >= 0 && idx < name.Length) {
				return(name[idx]);
			}
			return(null);
		}
		override public string SENS_UNIT(int idx)
		{
			string[] unit = {"[g]", "[g]", "[g]", "[deg/s]", "[deg/s]", "[deg/s]"};
			if (idx >= 0 && idx < unit.Length) {
				return(unit[idx]);
			}
			return(null);
		}
		override public int SENS_PREC(int idx)
		{
			int[] prec = {2, 2, 2, 2, 2, 2};
			if (idx >= 0 && idx < prec.Length) {
				return(prec[idx]);
			}
			return(0);
		}
	}
}
