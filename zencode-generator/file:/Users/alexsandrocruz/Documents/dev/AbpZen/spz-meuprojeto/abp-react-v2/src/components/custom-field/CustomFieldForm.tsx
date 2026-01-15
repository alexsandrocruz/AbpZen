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
  import { useCreateCustomField, useUpdateCustomField } from "@/lib/abp/hooks/useCustomFields";
import { toast } from "sonner";

const formSchema = z.object({
  
    entityType: z.any(),
  
    label: z.any(),
  
    fieldType: z.any(),
  
    fieldKey: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface CustomFieldFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function CustomFieldForm({
  isOpen,
  onClose,
  initialValues,
}: CustomFieldFormProps) {
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

  const createMutation = useCreateCustomField();
const updateMutation = useUpdateCustomField();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("CustomField updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("CustomField created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save customfield:", error);
    toast.error(error.message || "Failed to save customfield");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit CustomField": "Create CustomField" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the customfield." : "Fill in the details to create a new customfield." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="entityType" > EntityType * </Label>

<Input id="entityType" {...register("entityType") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="label" > Label * </Label>

<Input id="label" {...register("label") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="fieldType" > FieldType * </Label>

<Input id="fieldType" {...register("fieldType") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="fieldKey" > FieldKey * </Label>

<Input id="fieldKey" {...register("fieldKey") } />

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
