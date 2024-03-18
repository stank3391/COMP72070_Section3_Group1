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

WebUI.navigateToUrl('https://localhost:7048/Home')

WebUI.click(findTestObject('Object Repository/DM_Chrome/Page_Astro - COMP72070_Section3_Group1/a_AstroMessage'))

WebUI.click(findTestObject('Object Repository/DM_Chrome/Page_AstroMessage - COMP72070_Section3_Group1/li_Stank'))

WebUI.setText(findTestObject('Object Repository/DM_Chrome/Page_AstroMessage - COMP72070_Section3_Group1/input_Typing_messageInput'), 
    'test hello')

WebUI.sendKeys(findTestObject('Object Repository/DM_Chrome/Page_AstroMessage - COMP72070_Section3_Group1/input_Typing_messageInput'), 
    Keys.chord(Keys.ENTER))

WebUI.closeBrowser()

