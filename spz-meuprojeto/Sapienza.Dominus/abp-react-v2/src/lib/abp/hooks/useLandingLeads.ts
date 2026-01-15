import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLandingLeadsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useLandingLeads(input: GetLandingLeadsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["landingLeads", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/landing-lead", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllLandingLeads() {
  return useQuery({
    queryKey: ["landingLeads", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/landing-lead", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLandingLead(id: string) {
  return useQuery({
    queryKey: ["landingLead", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/landing-lead/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLandingLead() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/landing-lead", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["landingLeads"] });
    },
  });
}

export function useUpdateLandingLead() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/landing-lead/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["landingLeads"] });
      queryClient.invalidateQueries({ queryKey: ["landingLead", data.id] });
    },
  });
}

export function useDeleteLandingLead() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/landing-lead/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["landingLeads"] });
    },
  });
}


