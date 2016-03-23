<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="7f087dec-3583-4f93-8ee0-51fb2144caa7" namespace="LecOnline.Core" xmlSchemaNamespace="urn:LecOnline.Core" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="LecOnlineConfigurationSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="lecOnline">
      <attributeProperties>
        <attributeProperty name="Phone" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="phone" isReadOnly="false" documentation="Company phone">
          <type>
            <externalTypeMoniker name="/7f087dec-3583-4f93-8ee0-51fb2144caa7/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Email" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="email" isReadOnly="false" documentation="Email for the website">
          <type>
            <externalTypeMoniker name="/7f087dec-3583-4f93-8ee0-51fb2144caa7/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Skype" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="skype" isReadOnly="false" documentation="Name of the Skype account which will be used for the communication">
          <type>
            <externalTypeMoniker name="/7f087dec-3583-4f93-8ee0-51fb2144caa7/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="NoReplyAddress" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="noReplyAddress" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/7f087dec-3583-4f93-8ee0-51fb2144caa7/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SendNotificationEmails" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="sendNotificationEmails" isReadOnly="false" defaultValue="false">
          <type>
            <externalTypeMoniker name="/7f087dec-3583-4f93-8ee0-51fb2144caa7/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationSection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>