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
  import { useCreateLeadMessageTemplate, useUpdateLeadMessageTemplate } from "@/lib/abp/hooks/useLeadMessageTemplates";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    workflowId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LeadMessageTemplateFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LeadMessageTemplateForm({
  isOpen,
  onClose,
  initialValues,
}: LeadMessageTemplateFormProps) {
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

  const createMutation = useCreateLeadMessageTemplate();
const updateMutation = useUpdateLeadMessageTemplate();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("LeadMessageTemplate updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("LeadMessageTemplate created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save leadmessagetemplate:", error);
    toast.error(error.message || "Failed to save leadmessagetemplate");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit LeadMessageTemplate": "Create LeadMessageTemplate" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the leadmessagetemplate." : "Fill in the details to create a new leadmessagetemplate." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="workflowId" > WorkflowId</Label>

<Input id="workflowId" {...register("workflowId") } />

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
