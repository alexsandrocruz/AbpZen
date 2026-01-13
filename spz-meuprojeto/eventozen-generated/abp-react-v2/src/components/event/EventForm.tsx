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
  import { useCreateEvent, useUpdateEvent } from "@/lib/abp/hooks/useEvents";
import { toast } from "sonner";

const formSchema = z.object({
  
    artistId: z.any(),
  
    clientId: z.any(),
  
    localPartnerId: z.any(),
  
    locationId: z.any(),
  
    title: z.any(),
  
    type: z.any(),
  
    status: z.any(),
  
    startDateTime: z.any(),
  
    endDateTime: z.any(),
  
    fee: z.any(),
  
    description: z.any(),
  
    taxPercentage: z.any(),
  
    taxValue: z.any(),
  
    contractType: z.any(),
  
    negotiationType: z.any(),
  
    hasConflict: z.any(),
  
    suggestedAlternativeArtistId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface EventFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function EventForm({
  isOpen,
  onClose,
  initialValues,
}: EventFormProps) {
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

  const createMutation = useCreateEvent();
const updateMutation = useUpdateEvent();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Event updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Event created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save event:", error);
    toast.error(error.message || "Failed to save event");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Event": "Create Event" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the event." : "Fill in the details to create a new event." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="artistId" > ArtistId * </Label>

<Input id="artistId" {...register("artistId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="clientId" > ClientId * </Label>

<Input id="clientId" {...register("clientId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="localPartnerId" > LocalPartnerId</Label>

<Input id="localPartnerId" {...register("localPartnerId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="locationId" > LocationId</Label>

<Input id="locationId" {...register("locationId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="title" > Title</Label>

<Input id="title" {...register("title") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="type" > Type</Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="status" > Status</Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="startDateTime" > StartDateTime * </Label>

<Input id="startDateTime" type = "date" {...register("startDateTime") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="endDateTime" > EndDateTime * </Label>

<Input id="endDateTime" type = "date" {...register("endDateTime") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="fee" > Fee</Label>

<Input id="fee" type = "number" step = "any" {...register("fee") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="description" > Description</Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="taxPercentage" > TaxPercentage</Label>

<Input id="taxPercentage" type = "number" step = "any" {...register("taxPercentage") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="taxValue" > TaxValue</Label>

<Input id="taxValue" type = "number" step = "any" {...register("taxValue") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="contractType" > ContractType</Label>

<Input id="contractType" {...register("contractType") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="negotiationType" > NegotiationType</Label>

<Input id="negotiationType" {...register("negotiationType") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="hasConflict" > HasConflict</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="hasConflict"
checked = { watch("hasConflict") }
onCheckedChange = {(checked) => setValue("hasConflict", !!checked)}
              />
  < label htmlFor = "hasConflict" className = "text-sm font-normal" >
    { watch("hasConflict") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="suggestedAlternativeArtistId" > SuggestedAlternativeArtistId</Label>

<Input id="suggestedAlternativeArtistId" {...register("suggestedAlternativeArtistId") } />

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
