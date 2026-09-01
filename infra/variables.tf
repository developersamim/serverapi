variable "location" {
  description = "Azure region for all resources"
  type        = string
  default     = "australiaeast"
}

variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
  default     = "rg-test"
}

variable "container_registry_name" {
  description = "Globally unique Azure Container Registry name"
  type        = string
  default     = "customercorecr"
}

variable "container_app_environment_name" {
  description = "Azure Container Apps environment name"
  type        = string
  default     = "serverapi-env"
}