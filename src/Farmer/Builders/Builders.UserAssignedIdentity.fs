[<AutoOpen>]
module Farmer.Builders.ManagedIdentity

open Farmer
open Farmer.Arm.ManagedIdentity
open Farmer.ManagedIdentity

type UserAssignedIdentityConfig =
    { Name : ResourceName
      Tags : Map<string, string> }
    interface IBuilder with
        member this.DependencyName = this.Name
        member this.BuildResources location =
            [
                /// UserAssignedIdentity ARM resource.
                {
                    Name = this.Name
                    Location = location
                    Tags = this.Tags
                }
            ]

type UserAssignedIdentityBuilder() =
    member _.Yield _ =
        { Name = ResourceName.Empty
          Tags = Map.empty }
    /// Sets the name of the user assigned identity.
    [<CustomOperation "name">]
    member __.Name(state:UserAssignedIdentityConfig, name) = { state with Name = ResourceName name }
    /// Adds tags to the user assigned identity.
    [<CustomOperation "add_tags">]
    member _.Tags(state:UserAssignedIdentityConfig, pairs) = 
      { state with 
          Tags = pairs |> List.fold (fun map (key,value) -> Map.add key value map) state.Tags }
    /// Adds a tag to the user assigned identity.
    [<CustomOperation "add_tag">]
    member this.Tag(state:UserAssignedIdentityConfig, key, value) = this.Tags(state, [ (key,value) ])

/// Builds a user assigned identity.
let userAssignedIdentity = UserAssignedIdentityBuilder()
