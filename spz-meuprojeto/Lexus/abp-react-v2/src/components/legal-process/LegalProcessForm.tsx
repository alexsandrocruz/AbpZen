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
  import { useCreateLegalProcess, useUpdateLegalProcess } from "@/lib/abp/hooks/useLegalProcesses";
import { toast } from "sonner";

const formSchema = z.object({
  
    processNumber: z.any(),
  
    title: z.any(),
  
    description: z.any(),
  
    dateOpened: z.any(),
  
    lawyerId: z.any(),
  
    clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LegalProcessFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LegalProcessForm({
  isOpen,
  onClose,
  initialValues,
}: LegalProcessFormProps) {
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

  const createMutation = useCreateLegalProcess();
const updateMutation = useUpdateLegalProcess();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("LegalProcess updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("LegalProcess created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save legalprocess:", error);
    toast.error(error.message || "Failed to save legalprocess");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit LegalProcess": "Create LegalProcess" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the legalprocess." : "Fill in the details to create a new legalprocess." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="processNumber" > ProcessNumber * </Label>

<Input id="processNumber" {...register("processNumber") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="title" > Title * </Label>

<Input id="title" {...register("title") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="description" > Description</Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dateOpened" > DateOpened * </Label>

<Input id="dateOpened" type = "date" {...register("dateOpened") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="lawyerId" > LawyerId * </Label>

<Input id="lawyerId" {...register("lawyerId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="clientId" > ClientId * </Label>

<Input id="clientId" {...register("clientId") } />

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
