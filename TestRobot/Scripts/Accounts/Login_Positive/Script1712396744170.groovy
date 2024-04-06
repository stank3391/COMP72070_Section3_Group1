import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser('')

WebUI.navigateToUrl('https://localhost:7048/')

WebUI.click(findTestObject('Object Repository/Astro/Page_Astro - COMP72070_Section3_Group1/a_Account'))

WebUI.click(findTestObject('Object Repository/Astro/Page_Astro - COMP72070_Section3_Group1/a_Login'))

WebUI.setText(findTestObject('Object Repository/Astro/Page_LoginAction - COMP72070_Section3_Group1/input_Username_username'), 
    'dsa')

WebUI.setEncryptedText(findTestObject('Object Repository/Astro/Page_LoginAction - COMP72070_Section3_Group1/input_Password_password'), 
    'chIKUAMiWPY=')

WebUI.click(findTestObject('Object Repository/Astro/Page_LoginAction - COMP72070_Section3_Group1/button_Login'))

WebUI.click(findTestObject('Object Repository/Astro/Page_Astro - COMP72070_Section3_Group1/a_Account'))

WebUI.click(findTestObject('Object Repository/Astro/Page_Astro - COMP72070_Section3_Group1/a_Hello dsa'))

WebUI.click(findTestObject('Object Repository/Astro/Page_Profile - COMP72070_Section3_Group1/h2_Hi, dsa'))

WebUI.closeBrowser()

