import { useState, useEffect } from "react";
import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Loader2 } from "lucide-react";

type EntityType = "CLIENT" | "PROJECT" | "PROPOSAL" | "INVOICE" | "LEAD" | "TASK";

interface CustomFieldsSectionProps {
  entityType: EntityType;
  entityId?: string;
  values: Record<string, any>;
  onChange: (values: Record<string, any>) => void;
}

interface CustomFieldDefinition {
  id: string;
  fieldKey: string;
  label: string;
  fieldType: string;
  required: boolean;
  placeholder?: string;
  helpText?: string;
  defaultValue?: string;
  options?: string[];
}

export function CustomFieldsSection({
  entityType,
  entityId,
  values,
  onChange,
}: CustomFieldsSectionProps) {
  const { currentWorkspace } = useAuth();

  const { data: definitions = [], isLoading: loadingDefinitions } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, entityType],
    queryFn: () =>
      api.getCustomFields(currentWorkspace!.id, entityType),
    enabled: !!currentWorkspace,
  });

  const { data: savedValues = [], isLoading: loadingValues } = useQuery({
    queryKey: ["customFieldValues", currentWorkspace?.id, entityType, entityId],
    queryFn: () =>
      api.getCustomFieldValues(currentWorkspace!.id, entityType, entityId!),
    enabled: !!currentWorkspace && !!entityId,
  });

  useEffect(() => {
    if (savedValues.length > 0 && definitions.length > 0) {
      const initialValues: Record<string, any> = {};
      savedValues.forEach((sv: any) => {
        const def = definitions.find((d: CustomFieldDefinition) => d.id === sv.definitionId);
        if (def) {
          if (def.fieldType === "CHECKBOX") {
            initialValues[def.fieldKey] = sv.valueBoolean ?? false;
          } else if (def.fieldType === "NUMBER" || def.fieldType === "CURRENCY") {
            initialValues[def.fieldKey] = sv.valueNumber ?? "";
          } else if (def.fieldType === "DATE") {
            initialValues[def.fieldKey] = sv.valueDate ? new Date(sv.valueDate).toISOString().split("T")[0] : "";
          } else if (def.fieldType === "MULTISELECT") {
            initialValues[def.fieldKey] = sv.valueJson ?? [];
          } else {
            initialValues[def.fieldKey] = sv.valueText ?? "";
          }
        }
      });
      if (Object.keys(initialValues).length > 0) {
        onChange({ ...values, ...initialValues });
      }
    }
  }, [savedValues, definitions]);

  useEffect(() => {
    if (!entityId && definitions.length > 0) {
      const defaults: Record<string, any> = {};
      definitions.forEach((def: CustomFieldDefinition) => {
        if (def.defaultValue && !values[def.fieldKey]) {
          if (def.fieldType === "CHECKBOX") {
            defaults[def.fieldKey] = def.defaultValue === "true";
          } else if (def.fieldType === "NUMBER" || def.fieldType === "CURRENCY") {
            defaults[def.fieldKey] = parseFloat(def.defaultValue) || "";
          } else if (def.fieldType === "MULTISELECT") {
            try {
              defaults[def.fieldKey] = JSON.parse(def.defaultValue);
            } catch {
              defaults[def.fieldKey] = [];
            }
          } else {
            defaults[def.fieldKey] = def.defaultValue;
          }
        }
      });
      if (Object.keys(defaults).length > 0) {
        onChange({ ...values, ...defaults });
      }
    }
  }, [definitions, entityId]);

  const handleChange = (fieldKey: string, value: any) => {
    onChange({ ...values, [fieldKey]: value });
  };

  if (loadingDefinitions) {
    return (
      <div className="flex items-center justify-center py-4">
        <Loader2 className="h-4 w-4 animate-spin" />
      </div>
    );
  }

  if (definitions.length === 0) {
    return null;
  }

  return (
    <div className="space-y-4">
      <h4 className="text-sm font-medium text-muted-foreground border-b pb-2">
        Campos Personalizados
      </h4>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {definitions.map((def: CustomFieldDefinition) => (
          <div key={def.id} className="space-y-2">
            <Label htmlFor={`custom-${def.fieldKey}`}>
              {def.label}
              {def.required && <span className="text-destructive ml-1">*</span>}
            </Label>

            {def.fieldType === "TEXT" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                placeholder={def.placeholder}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.fieldType === "TEXTAREA" && (
              <Textarea
                id={`custom-${def.fieldKey}`}
                data-testid={`textarea-custom-${def.fieldKey}`}
                placeholder={def.placeholder}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
                rows={3}
              />
            )}

            {def.fieldType === "NUMBER" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                type="number"
                placeholder={def.placeholder}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.fieldType === "CURRENCY" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                type="number"
                step="0.01"
                placeholder={def.placeholder || "0,00"}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.fieldType === "DATE" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                type="date"
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.fieldType === "CHECKBOX" && (
              <div className="flex items-center space-x-2 pt-2">
                <Checkbox
                  id={`custom-${def.fieldKey}`}
                  data-testid={`checkbox-custom-${def.fieldKey}`}
                  checked={values[def.fieldKey] || false}
                  onCheckedChange={(checked) => handleChange(def.fieldKey, checked)}
                />
                <label
                  htmlFor={`custom-${def.fieldKey}`}
                  className="text-sm text-muted-foreground cursor-pointer"
                >
                  {def.placeholder || "Sim"}
                </label>
              </div>
            )}

            {def.fieldType === "SELECT" && (
              <Select
                value={values[def.fieldKey] || ""}
                onValueChange={(value) => handleChange(def.fieldKey, value)}
              >
                <SelectTrigger data-testid={`select-custom-${def.fieldKey}`}>
                  <SelectValue placeholder={def.placeholder || "Selecione..."} />
                </SelectTrigger>
                <SelectContent>
                  {(def.options || []).map((option) => (
                    <SelectItem key={option} value={option}>
                      {option}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            )}

            {def.fieldType === "MULTISELECT" && (
              <div className="flex flex-wrap gap-2 p-2 border rounded-md min-h-[38px]">
                {(def.options || []).map((option) => {
                  const selected = (values[def.fieldKey] || []).includes(option);
                  return (
                    <button
                      key={option}
                      type="button"
                      data-testid={`multiselect-option-${def.fieldKey}-${option}`}
                      onClick={() => {
                        const current = values[def.fieldKey] || [];
                        if (selected) {
                          handleChange(def.fieldKey, current.filter((v: string) => v !== option));
                        } else {
                          handleChange(def.fieldKey, [...current, option]);
                        }
                      }}
                      className={`px-2 py-1 text-xs rounded-full transition-colors ${
                        selected
                          ? "bg-primary text-primary-foreground"
                          : "bg-muted text-muted-foreground hover:bg-muted/80"
                      }`}
                    >
                      {option}
                    </button>
                  );
                })}
              </div>
            )}

            {def.fieldType === "URL" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                type="url"
                placeholder={def.placeholder || "https://"}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.fieldType === "EMAIL" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                type="email"
                placeholder={def.placeholder || "email@exemplo.com"}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.fieldType === "PHONE" && (
              <Input
                id={`custom-${def.fieldKey}`}
                data-testid={`input-custom-${def.fieldKey}`}
                type="tel"
                placeholder={def.placeholder || "(00) 00000-0000"}
                value={values[def.fieldKey] || ""}
                onChange={(e) => handleChange(def.fieldKey, e.target.value)}
              />
            )}

            {def.helpText && (
              <p className="text-xs text-muted-foreground">{def.helpText}</p>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
