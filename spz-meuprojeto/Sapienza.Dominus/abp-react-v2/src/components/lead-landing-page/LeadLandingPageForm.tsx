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
  import { useCreateLeadLandingPage, useUpdateLeadLandingPage } from "@/lib/abp/hooks/useLeadLandingPages";
import { toast } from "sonner";

const formSchema = z.object({
  
    title: z.any(),
  
    slug: z.any(),
  
    workflowId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LeadLandingPageFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LeadLandingPageForm({
  isOpen,
  onClose,
  initialValues,
}: LeadLandingPageFormProps) {
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

  const createMutation = useCreateLeadLandingPage();
const updateMutation = useUpdateLeadLandingPage();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("LeadLandingPage updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("LeadLandingPage created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save leadlandingpage:", error);
    toast.error(error.message || "Failed to save leadlandingpage");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit LeadLandingPage": "Create LeadLandingPage" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the leadlandingpage." : "Fill in the details to create a new leadlandingpage." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="title" > Title * </Label>

<Input id="title" {...register("title") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="slug" > Slug * </Label>

<Input id="slug" {...register("slug") } />

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
