import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateCustomFieldValue, useUpdateCustomFieldValue } from "@/lib/abp/hooks/useCustomFieldValues";
import { toast } from "sonner";

const formSchema = z.object({
  
    entityId: z.any(),
  
    valueText: z.any(),
  
    fieldId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface CustomFieldValueFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function CustomFieldValueForm({
  isOpen,
  onClose,
  initialValues,
}: CustomFieldValueFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateCustomFieldValue();
const updateMutation = useUpdateCustomFieldValue();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("CustomFieldValue updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("CustomFieldValue created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save customfieldvalue:", error);
    toast.error(error.message || "Failed to save customfieldvalue");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit CustomFieldValue": "Create CustomFieldValue" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the customfieldvalue." : "Fill in the details to create a new customfieldvalue." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="entityId" > EntityId * </Label>

<Input id="entityId" {...register("entityId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valueText" > ValueText</Label>

<Input id="valueText" {...register("valueText") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="fieldId" > FieldId</Label>

<Input id="fieldId" {...register("fieldId") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
